using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZ.Clases.DataClases.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.WorkClases
{
    /// <summary>
    /// Основной рабочий класс
    /// </summary>
    internal class MainWork : IDisposable
    {
        /// <summary>
        /// Список задач по созданию хешей
        /// </summary>
        List<Task<CreateHashTask>> taskList;
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
            //Инициализируем дефолтные значения
            taskList = new List<Task<CreateHashTask>>();
            isWork = true;
            //Запускаем основной рабочий поток
            new Thread(Main).Start();
        }

        /// <summary>
        /// Основной рабочий класс
        /// </summary>
        private void Main()
        {
            int oldCount = 0;
            do
            {
                //Если есть задачи
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
                //Метод отправки ивента обновления статуса
                SendUpdateStatusEvent(ref oldCount);
                //Цикл идёт пока работает программа
            } while (isWork);
        }

        /// <summary>
        /// Обновляем список задач
        /// </summary>
        private void UpdateTasks()
        {
            //Удаляем завершённые задачи
            RemoveCompletedTasks();
            //Получаем количество выполняющихся задач
            int count = GetCountActiveTasks();
            //Получаем количество слотов под параллельно выполняющиеся задачи
            count = Constants.SUNCHRONOUS_TASKS_COUNT - count;
            //Если слоты есть
            if (count > 0)
                //Запускаем задачи
                StartTasks(count);
        }

        /// <summary>
        /// Метод отправки ивента обновления статуса
        /// </summary>
        /// <param name="oldCount">Старое количество задач в списке</param>
        private void SendUpdateStatusEvent(ref int oldCount)
        {
            //Если количество задач в списке изменилось
            if(taskList.Count != oldCount)
            {
                //Обновляем старое количество задач
                oldCount = taskList.Count;
                //Вызываем ивент обновления работы
                DCTHash.InvokeUpdateCreationStatus(taskList.Count);
            }
        }

        /// <summary>
        /// Удаляем завершённые задачи
        /// </summary>
        private void RemoveCompletedTasks() =>
            taskList.RemoveAll(task => task.Status == TaskStatus.RanToCompletion);

        /// <summary>
        /// Получаем количество выполняющихся задач
        /// </summary>
        /// <returns>Количество задач в работе</returns>
        private int GetCountActiveTasks() =>
            taskList.Count(task => task.Status == TaskStatus.Running);

        /// <summary>
        /// Запускаем задачи на выполнение
        /// </summary>
        /// <param name="count">Количество задач для выполнения</param>
        private void StartTasks(int count) =>
            //Получаем из списка задач
            taskList
                //Задачи ждущие выполнения
                .Where(task => task.Status == TaskStatus.Created)
                //Берём из них первые, по количеству слотов
                .Take(count)
                //В виде списка
                .ToList()
                //И запускаем их
                .ForEach(task => task.Start());

        /// <summary>
        /// Формируем хеш файла
        /// </summary>
        /// <param name="path">ПУть к файлу на диске</param>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <returns>Результат генерации хеша</returns>
        private CreateHashTask CalculateHash(string path, bool isNeedMedianFilter)
        {
            //ПОлучаем информацию о файле
            FileInfo fi = new FileInfo(path);
            //Инициализируем информацию о задаче
            CreateHashTask task = new CreateHashTask() {
                FileName = fi.Name,
                Path = fi.DirectoryName,
                IsNeedMedianFilter = isNeedMedianFilter
            };
            //Инициализируем класс работы с изображением
            ImageWork imageWork = new ImageWork();
            //Рассчитываем хеш файла
            imageWork.CalculateHash(task);
            //Возвращаем результат
            return task;
        }

        /// <summary>
        /// Формируем хеш файла
        /// </summary>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <param name="image">Ссылка на загруженную информацию об изображении</param>
        /// <returns>Результат генерации хеша</returns>
        private CreateHashTask CalculateHash(ByteImageInfo image, bool isNeedMedianFilter)
        {
            //Инициализируем информацию о задаче
            CreateHashTask task = new CreateHashTask() {
                IsNeedMedianFilter = isNeedMedianFilter
            };
            //Инициализируем класс работы с изображением
            ImageWork imageWork = new ImageWork();
            //Рассчитываем хеш файла
            imageWork.CalculateHash(task, image);
            //Возвращаем результат
            return task;
        }


        /// <summary>
        /// Создаём таску на генерацию хеша
        /// </summary>
        /// <param name="path">ПУть к файлу на диске</param>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <returns>Задача по генерации хеша</returns>
        private Task<CreateHashTask> CreateCalculateHashTask(string path, bool isNeedMedianFilter) =>
            new Task<CreateHashTask>(() => CalculateHash(path, isNeedMedianFilter));

        /// <summary>
        /// Создаём таску на генерацию хеша
        /// </summary>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <param name="image">Ссылка на загруженную информацию об изображении</param>
        /// <returns>Задача по генерации хеша</returns>
        private Task<CreateHashTask> CreateCalculateHashTask(ByteImageInfo image, bool isNeedMedianFilter) =>
            new Task<CreateHashTask>(() => CalculateHash(image, isNeedMedianFilter));

        /// <summary>
        /// Добавляем задачи для генерации хешей
        /// </summary>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <param name="pathList">Список путей к файлам изображений</param>
        /// <returns>Список задач по генерации хешей</returns>
        public List<Task<CreateHashTask>> AddTasksAsync(List<string> pathList, bool isNeedMedianFilter)
        {
            //Конвертируем список файлов в список задач по генерации хешей
            List<Task<CreateHashTask>> tasks = pathList
                .ConvertAll(file => CreateCalculateHashTask(file, isNeedMedianFilter));
            //Лочим общий список задач
            lock (taskList)
                //Добавляем задачу в список
                taskList.AddRange(tasks);
            //Возвращаем зщадачу из списка
            return tasks;
        }

        /// <summary>
        /// Добавляем задачу для генерации хешей по загруженной картинке
        /// </summary>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <param name="image">Ссылка на загруженную информацию об изображении</param>
        /// <returns>Задача по генерации хешей</returns>
        public Task<CreateHashTask> AddTaskAsync(ByteImageInfo image, bool isNeedMedianFilter)
        {
            //Конвертируем список файлов в список задач по генерации хешей
            Task<CreateHashTask> task = CreateCalculateHashTask(image, isNeedMedianFilter);
            //Лочим общий список задач
            lock (taskList)
                //Добавляем задачу в список
                taskList.Add(task);
            //Возвращаем зщадачу из списка
            return task;
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
