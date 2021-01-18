using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DCTHashZDemo.Content
{
    /// <summary>
    /// Класс реализующий выполнение первого демо примера
    /// </summary>
    internal class FirstDemoExample
    {

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FirstDemoExample()
        {

        }


        /// <summary>
        /// Обработчик события обновления статусов
        /// </summary>
        /// <param name="waitCount">Количество ожидающих выполнения задач</param>
        private void DCTHash_UpdateCreationStatus(int waitCount) =>
            //Тут просто вывожу статус загрузки
            Console.WriteLine($"Wait tasks: {waitCount};");



        /// <summary>
        /// Метод отображения сгенерированных хешей
        /// </summary>
        /// <param name="tasks">Список завершённых задач</param>
        private void WriteResult(List<Task<CreateHashTask>> tasks)
        {
            CreateHashTask result;
            //Тут просто вывожу результаты создания хешей
            Console.WriteLine("Hash list creation complete!");
            //Проходимся по задачам
            foreach (var task in tasks)
            {
                //Получаем результат выполнения задачи
                result = task.Result;
                //Выводим его в консоль
                Console.WriteLine($"\t[Status: {result.Status}] " +
                    $"[Hash: {result.Hash.GetValueOrDefault(0)}] " +
                    $"[FileName: {result.FileName}]");
            }
        }

        


        /// <summary>
        /// Первый вариант демо-программы
        /// </summary>
        /// <param name="scanPath">Путь для сканирования</param>
        public void Start(string scanPath)
        {
            //ВЫводим дополнительную инфу в консоль
            Console.WriteLine("First demo example was started.");
            //Добавляем обработчик события обновления статусов
            DCTHash.UpdateCreationStatus += DCTHash_UpdateCreationStatus;
            //Инициализируем класс работы с хешем
            using (DCTHash hash = new DCTHash())
            {
                //ПОлучаем список файлов из директории сканирования
                List<string> paths = Directory.GetFiles(scanPath).ToList();
                //ВЫводим дополнительную инфу в консоль
                Console.WriteLine($"Find {paths.Count} files.");
                Console.WriteLine($"Hash generation started.");
                //Добавляем задачи генерации хешей
                List<Task<CreateHashTask>> tasks = hash.AddTasksAsync(paths, false);
                //Переходим к новой строке
                Console.WriteLine();
                //Ждём завершения всех задач
                Task.WaitAll(tasks.ToArray(), -1);
                //ВЫводим на экран результат генерации
                WriteResult(tasks);

                //Запрещаем закрытие программы до 
                //нажатия кнопки "Enter"
                Console.ReadLine();
            }
        }
    }
}
