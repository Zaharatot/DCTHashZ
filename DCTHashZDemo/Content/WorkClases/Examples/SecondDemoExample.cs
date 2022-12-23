using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
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
    /// Класс реализующий выполнение второго демо примера
    /// </summary>
    internal class SecondDemoExample
    {
        /// <summary>
        /// Класс работы с хешами
        /// </summary>
        private DCTHash hash;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public SecondDemoExample()
        {
            //Инициализируем используемые классы
            hash = new DCTHash();
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
        /// Выполняем сравнение полученных хешей
        /// </summary>
        /// <param name="taskList">Список завершённых задач</param>
        private void EqualResults(List<CreateHashTask> taskList)
        {
            ulong taskHash, fileHash;
            bool eqal;
            //Проходимся по файлам получившим хеши
            foreach(var task in taskList)
            {
                //Получаем хеш задачи
                taskHash = task.Hash.GetValueOrDefault(0);
                //ВЫводим общую инфу о результате сравнения
                Console.WriteLine($"[FileName: {task.FileName}] " +
                    $"[Status: {task.Status}] " +
                    $"[Hash: {taskHash}]");
                //Если хеш был получен корректно
                if (task.Status == CreateHashStatuses.Complete)
                {
                    //Для каждого из них выполняем сравнение
                    foreach (var file in taskList)
                    {
                        //Если это не тот же файл
                        if(file.FileName != task.FileName)
                        {
                            //Если хеш файла был успешно получен
                            if (file.Status == CreateHashStatuses.Complete) 
                            {
                                //ПОлучаем хеш файла
                                fileHash = file.Hash.GetValueOrDefault(0);
                                //Сравниваем хеши
                                eqal = hash.EqalImageHash(taskHash, fileHash);
                                //ПРоставляем цвет пор равеноству
                                Console.ForegroundColor = (eqal) ? ConsoleColor.Green : ConsoleColor.Red;                                
                                //Выводим инфу о сравнении
                                Console.WriteLine($"\t [FileName: {file.FileName}] " +
                                    $"[Hash: {file.Hash.GetValueOrDefault(0)}] " +
                                    $"[Similarity: {hash.GetHashSimilarity(taskHash, fileHash)}] " +
                                    $"[Equality: {eqal}]");
                                //Сбрасываем цвет
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            //Если у этого файла нет хеша
                            else
                                //Просто выводим статус
                                Console.WriteLine($"\t [FileName: {file.FileName}] [Status: {file.Status}]");                            
                        }
                    }
                }
            }
        }
        


        /// <summary>
        /// Первый вариант демо-программы
        /// </summary>
        /// <param name="scanPath">Путь для сканирования</param>
        public void Start(string scanPath)
        {
            //ВЫводим дополнительную инфу в консоль
            Console.WriteLine("Second demo example was started.");
            
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
            //Получаем список результатов выполнения задач
            List<CreateHashTask> results = tasks
                .Select(task => task.Result).ToList();
            //ВЫполняем сравнение и вывод результатов
            EqualResults(results);

            //Запрещаем закрытие программы до 
            //нажатия кнопки "Enter"
            Console.ReadLine();
            //Завершаем работу с хешами
            hash.Dispose();
        }
    }
}
