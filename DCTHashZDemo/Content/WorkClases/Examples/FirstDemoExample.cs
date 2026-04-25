using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCTHashZDemo.Content.WorkClases.Examples
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
        /// <param name="results">Список результатов генерации</param>
        private void WriteResult(List<CreateHashTask> results)
        {
            //Тут просто вывожу результаты создания хешей
            Console.WriteLine("Hash list creation complete!");
            //Проходимся по результатам
            foreach (var result in results)
            {
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
        public async Task StartAsync(string scanPath)
        {
            //ВЫводим дополнительную инфу в консоль
            Console.WriteLine("First demo example was started.");
            //Добавляем обработчик события обновления статусов
            DCTHash.UpdateCreationStatus += DCTHash_UpdateCreationStatus;
            try
            {
                //Инициализируем класс работы с хешем
                using (DCTHash hash = new DCTHash())
                {
                    //ПОлучаем список файлов из директории сканирования
                    List<string> paths = Directory.GetFiles(scanPath).ToList();
                    //ВЫводим дополнительную инфу в консоль
                    Console.WriteLine($"Find {paths.Count} files.");
                    Console.WriteLine($"Hash generation started.");
                    //Запускаем генерацию хешей
                    List<CreateHashTask> results = await hash
                        .CalculateHashesAsync(paths, false)
                        .ConfigureAwait(false);
                    //Переходим к новой строке
                    Console.WriteLine();
                    //ВЫводим на экран результат генерации
                    WriteResult(results);

                    //Запрещаем закрытие программы до 
                    //нажатия кнопки "Enter"
                    Console.ReadLine();
                }
            }
            finally
            {
                DCTHash.UpdateCreationStatus -= DCTHash_UpdateCreationStatus;
            }
        }
    }
}
