using DCTHashZ.Clases.DataClases;
using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZ.Clases.DataClases.Other;
using DCTHashZ.Clases.WorkClases.Filters;
using DCTHashZ.Clases.WorkClases.HashWork;
using DCTHashZ.Clases.WorkClases.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DCTHashZ.Clases.DataClases.Other.Enums;

namespace DCTHashZ.Clases.WorkClases
{
    /// <summary>
    /// Основной рабочий класс приложения
    /// </summary>
    internal class MainWork : IDisposable
    {

        /// <summary>
        /// Список задач по генерации хешей
        /// </summary>
        private List<CreateHashTask> taskList;
        /// <summary>
        /// Список классов генерации хеша
        /// </summary>
        private List<ImageWork> workersList;
        /// <summary>
        /// Флаг выполнения работы классом
        /// </summary>
        private bool isWork;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MainWork()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            InitWorkers();
            //Инициализируем дефолтные значения
            taskList = new List<CreateHashTask>();
            isWork = true;
            //Запускаем основной рабочий поток
            new Thread(Main).Start();
        }

        /// <summary>
        /// Инициализируем список рабочих классов
        /// </summary>
        private void InitWorkers()
        {
            //Инициализируем список воркеров
            workersList = new List<ImageWork>();
            //Добавляем рабочие классы в список
            for (int i = 0; i < Constants.SUNCHRONOUS_TASKS_COUNT; i++)
                workersList.Add(new ImageWork());
        }


        /// <summary>
        /// Основной рабочий класс
        /// </summary>
        private void Main()
        {
            do
            {
                //Если ексть задачи
                if (taskList.Count > 0)
                {
                    //Обновляем работу с ними                    
                    UpdateTasks();
                    //Спим указанное время
                    Thread.Sleep(Constants.UPDATE_TASK_STATUSES_DELAY);
                }
                //Если задач нет
                else
                    //Спим подольше
                    Thread.Sleep(Constants.WAIT_TASKS_DELAY);
                //Цикл идёт пока работает программа
            } while (isWork);
        }

        /// <summary>
        /// Обновляем список задач
        /// </summary>
        private void UpdateTasks()
        {
            //Получем список свободных рабочих классов
            List<ImageWork> freeWorkers = GetFreeWorkers();
            //Запускаем генерацию хешей
            StartWorkers(freeWorkers);
            //Вызываем ивенты обновления статуса, и 
            //если была завершена
            if (SendStatusEvents())
                //Очищаем список задач
                taskList.Clear();
        }

        /// <summary>
        /// Вызываем события обновления статуса
        /// </summary>
        /// <returns>True - работа была завершена</returns>
        private bool SendStatusEvents()
        {
            //Получаем количество завершённых задач
            int completed = taskList.Count(task => task.IsCompleteStatus());
            //Если все задачи находятся в завершённом статусе
            //то вся работа над генерацией хешей была завершена
            bool isComplete = (completed == taskList.Count);
            //Вызываем ивент обновленяи стауса
            SendStatusEvent(completed);
            //Если работа была завершена
            if (isComplete)
                //ВЫзываем ивент завершения загрузки
                SendCompleteEvent();
            return isComplete;
        }

        /// <summary>
        /// Отправляем ивент обновления статуса
        /// </summary>
        /// <param name="completed">Количество завершённых задач</param>
        private void SendStatusEvent(int completed)
        {
            //Получаем количество ожидающих запуска задач
            int waitCount = taskList.Count(task => task.IsWaitStatus());
            //Вычисляем аоличество задач находящихся "в работе"
            int inWorkCount = taskList.Count - waitCount - completed;
            //Получаем общее количество задач
            int count = taskList.Count;
            //Ивенты вызываем в отдельном потоке, т.к. их обработка 
            //будет задерживать работу основного потока этого класса
            new Thread(() => {
                //Вызываем ивент обновления статусов
                DCTHash.InvokeUpdateCreationStatus(
                    waitCount,
                    inWorkCount,
                    completed,
                    count
               );
            }).Start();
        }

        /// <summary>
        /// Вызываем событие завершения формирования списка хешей
        /// </summary>
        private void SendCompleteEvent()
        {
            //Создаём копию списка задач. Это делается для того,
            //чтобы очистка основного списка задач не затронула
            //передаваемое вне библиотеки значение
            List<CreateHashTask> copyTaskList = new List<CreateHashTask>(taskList);
            //Ивенты вызываем в отдельном потоке, т.к. их обработка 
            //будет задерживать работу основного потока этого класса
            new Thread(() => {
                //Вызываем ивент завершения генерации хешей
                DCTHash.InvokeCompleteCeneration(copyTaskList);
            }).Start();
        }



        /// <summary>
        /// Создаём новые классы генерации хеша
        /// </summary>
        /// <param name="freeWorkers">Список свободных воркеров</param>
        private void StartWorkers(List<ImageWork> freeWorkers)
        {
            //Получаем задачи ожидающие запуска
            List<CreateHashTask> tasks = GetWaitingTasks(freeWorkers.Count);
            //ПОлучаем меньшее значение из количества воркеров и задач
            int count = Math.Min(tasks.Count, freeWorkers.Count);
            //Проходимся по задачам
            for (int i = 0; i < count; i++)
            {
                //Проставляем задаче статус "В работе"
                tasks[i].Status = CreateHashStatuses.Creation;
                //Раздаём воркерам задачи
                freeWorkers[i].CalculateHash(tasks[i]);
            }
        }

        /// <summary>
        /// Получчаем список задач, ожидающих выполнения
        /// </summary>
        /// <param name="slots">КОличество слотов для создания</param>
        /// <returns>Список задач для выполнения</returns>
        private List<CreateHashTask> GetWaitingTasks(int slots) =>
            //Берём из списка задач
            taskList               
                //Все активные задачи
                .Where(task => task.IsWaitStatus())
                //В количестве не большем чем количество слотов
                .Take(slots)
                //В виде списка
                .ToList();

        /// <summary>
        /// Получем список свободных рабочих классов
        /// </summary>
        /// <returns>Список свободных воркеров</returns>
        private List<ImageWork> GetFreeWorkers() =>
            workersList.Where(worker => !worker.IsWork).ToList();

        /// <summary>
        /// Генерируем список задач по списку путей
        /// </summary>
        /// <param name="paths">Список путей к файлам изображений</param>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <returns>Спаисок задач</returns>
        private List<CreateHashTask> GenerateTasksFromPaths(List<string> paths, bool isNeedMedianFilter)
        {
            List<CreateHashTask> ex = new List<CreateHashTask>();
            FileInfo buff;
            //Проходимся по списку путей
            foreach(var path in paths)
            {
                //ПОлучаем информацию о файле
                buff = new FileInfo(path);
                //ИНициализируем задачу по информации о файле
                ex.Add(new CreateHashTask() { 
                    FileName = buff.Name,
                    Path = buff.DirectoryName,
                    IsNeedMedianFilter = isNeedMedianFilter
                });
            }
            return ex;
        }


        /// <summary>
        /// Добавляем задачи для генерации хешей
        /// </summary>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <param name="paths">Список путей к файлам изображений</param>
        public void AddTasks(List<string> paths, bool isNeedMedianFilter)
        {
            //Добавляем задачи в общий список
            lock (this.taskList)
            {
                this.taskList.AddRange(
                    GenerateTasksFromPaths(paths, isNeedMedianFilter)
                );
            }
        }

        /// <summary>
        /// Метод очистки неуправляемых ресмурсов класса
        /// </summary>
        public void Dispose()
        {
            //Завершаем работу класса
            isWork = false;
        }
    }
}
