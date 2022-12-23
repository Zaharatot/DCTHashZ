using DCTHashZDemo.Content;
using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DCTHashZDemo.Content.WorkClases.Examples;

namespace DCTHashZDemo
{
    class Program
    {

        /// <summary>
        /// Основной метод программы
        /// </summary>
        static void Main(string[] args)
        {
            //Выводим стартовое сообщение, и получаем номер режима
            int messageType = SendStartMessage();
            //Путь для сканирования
            string scanPath = GetFilesPathMessage();
            //ВЫбираем пример по типу
            switch (messageType)
            {
                case 1:
                    {
                        //Вызываем работу первого примера
                        new FirstDemoExample().Start(scanPath);
                        break;
                    }
                case 2:
                    {
                        //Вызываем работу второго примера
                        new SecondDemoExample().Start(scanPath);
                        break;
                    }
                case 3:
                    {
                        //Вызываем работу третьего примера
                        new ThridDemoExample().Start(scanPath);
                        break;
                    }
            }

        }

        /// <summary>
        /// Выводим сообщение о необходимости вывода пути
        /// и возвращаем строку введённого пути
        /// </summary>
        /// <returns>Строка пути для сканирования</returns>
        private static string GetFilesPathMessage()
        {
            //Выводим сообщение о запросе пути а потом
            //считываем и возвращаем результат
            Console.Write("Enter scan path: ");
            return Console.ReadLine();
        }

        /// <summary>
        /// ВЫводим стартовое сообщение
        /// </summary>
        /// <returns>Номер выбранного типа демо-режима</returns>
        private static int SendStartMessage()
        {
            int ex = 1;
            //Выводим приветственное сообщение
            Console.WriteLine("DCTHashZ Demo started.");
            Console.WriteLine("Select Demo type:");
            Console.WriteLine("\t1. Hash visualize");
            Console.WriteLine("\t2. Check equality");
            Console.WriteLine("\t3. Calcaulte hash from preloaded image");
            Console.Write("Enter type number (1-3): ");
            //Считываем введённые данные
            string result = Console.ReadLine();
            //Парсим результат, и в случае ошибки выбираем дефолтный режим
            if (!int.TryParse(result, out ex))
                ex = 1;
            return ex;
        }

    }
}
