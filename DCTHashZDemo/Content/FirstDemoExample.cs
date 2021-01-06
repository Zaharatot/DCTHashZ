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
        /// <param name="inWorkCount">Количество задач в работе</param>
        /// <param name="completeCount">Количество завершённых задач</param>
        /// <param name="count">Общее количество задач</param>
        private void DCTHash_UpdateCreationStatus(int waitCount,
            int inWorkCount, int completeCount, int count)
        {
            //Тут просто вывожу статус загрузки
            Console.WriteLine($"Wait: {waitCount}; InWork: {inWorkCount}; " +
                $"Completed: {completeCount}; FullCount: {count};");
        }

        /// <summary>
        /// Обработчик события завершения создания хешей
        /// </summary>
        /// <param name="taskList">Список завершённых задач</param>
        private void DCTHash_CompleteCeneration(List<CreateHashTask> taskList)
        {
            //Тут просто вывожу результаты создания хешей
            Console.WriteLine("Hash list creation complete!");
            foreach (var task in taskList)
                Console.WriteLine($"\t[Status: {task.Status}] " +
                    $"[Hash: {task.Hash.GetValueOrDefault(0)}] " +
                    $"[FileName: {task.FileName}]");
        }

        


        /// <summary>
        /// Первый вариант демо-программы
        /// </summary>
        /// <param name="scanPath">Путь для сканирования</param>
        public void Start(string scanPath)
        {
            //ВЫводим дополнительную инфу в консоль
            Console.WriteLine("First demo example was started.");
            //Добавляем обработчик события завершения создания хешей
            DCTHash.CompleteCeneration += DCTHash_CompleteCeneration;
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
                //Добавляем задачу сканирования
                hash.AddTasks(paths, false);
                //Переходим к новой строке
                Console.WriteLine();

                //Запрещаем закрытие программы до 
                //нажатия кнопки "Enter"
                Console.ReadLine();
            }
        }
    }
}
