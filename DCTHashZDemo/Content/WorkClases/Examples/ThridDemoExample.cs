using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZDemo.Content.DataClases;
using DCTHashZDemo.Content.WorkClases.Load;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DCTHashZ.Clases.DataClases.Global.Enums;

namespace DCTHashZDemo.Content.WorkClases.Examples
{
    /// <summary>
    /// Класс реализующий выполнение третьего демо примера
    /// </summary>
    internal class ThridDemoExample
    {
        /// <summary>
        /// Класс работы с хешами
        /// </summary>
        private DCTHash _hash;
        /// <summary>
        /// Класс загрузки пикселей
        /// </summary>
        private LoadImagePixels _loadImagePixels;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ThridDemoExample()
        {
            //Инициализируем используемые классы
            _hash = new DCTHash();
            _loadImagePixels = new LoadImagePixels();
            //Добавляем обработчик события обновления статусов
            DCTHash.UpdateCreationStatus += DCTHash_UpdateCreationStatus;                   
        }


        /// <summary>
        /// Обработчик события обновления статусов
        /// </summary>
        /// <param name="waitCount">Количество ожидающих выполнения задач</param>
        private void DCTHash_UpdateCreationStatus(int waitCount) =>
            //Тут просто вывожу статус загрузки
            Console.WriteLine($"Wait tasks: {waitCount};");

        /// <summary>
        /// Метод загрузки списка изображений
        /// </summary>
        /// <param name="scanPath">Путь для загрузки изображений</param>
        /// <returns>Список изображений для загрузки</returns>
        private List<ByteImageInfo> LoadImages(string scanPath)
        {
            //Получаем список файлов из директории сканирования
            List<string> paths = Directory.GetFiles(scanPath).ToList();
            //Выполняем загрузку всех изображений
            return paths.ConvertAll(path => _loadImagePixels.LoadImage(path)).ToList();
        }

        /// <summary>
        /// Создаём задачи на генерацию хешей для картинок
        /// </summary>
        /// <param name="images">Картинки для генерации</param>
        /// <returns>Список задач по генерации хешей</returns>
        private List<Task<CreateHashTask>> CreateGenerateTasks(List<ByteImageInfo> images) =>
            images.ConvertAll(image => _hash.AddTaskAsync(image));

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
        /// Третий вариант демо-программы
        /// </summary>
        /// <param name="scanPath">Путь для сканирования</param>
        public void Start(string scanPath)
        {
            //ВЫводим дополнительную инфу в консоль
            Console.WriteLine("Thrid demo example was started.");
            //Выполняем загрузку всех изображений
            List<ByteImageInfo> images = LoadImages(scanPath);
            //ВЫводим дополнительную инфу в консоль
            Console.WriteLine($"Find {images.Count} files.");
            Console.WriteLine($"Hash generation started.");
            //Добавляем задачи генерации хешей
            List<Task<CreateHashTask>> tasks = CreateGenerateTasks(images);
            //Переходим к новой строке
            Console.WriteLine();
            //Ждём завершения всех задач
            Task.WaitAll(tasks.ToArray(), -1);
            //ВЫводим на экран результат генерации
            WriteResult(tasks);

            //Запрещаем закрытие программы до 
            //нажатия кнопки "Enter"
            Console.ReadLine();
            //Завершаем работу с хешами
            _hash.Dispose();
        }
    }
}
