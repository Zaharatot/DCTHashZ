using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DCTHashZ.Clases.DataClases.Other.Enums;

namespace DCTHashZ.Clases.DataClases.ImageWork
{
    /// <summary>
    /// Класс описания задачи по созданию хеша файла
    /// </summary>
    public class CreateHashTask
    {
        /// <summary>
        /// Путь к папке с файлом
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Хеш файла
        /// </summary>
        public ulong? Hash { get; set; }
        /// <summary>
        ///  Данный флаг активирует медианную фильтрацию, при выполнении упрощения изображения. 
        ///  ВНИМАНИЕ! Хоть медианная фильтрация и добавляет некоторый процент точности при 
        ///  итоговом сравнении хешей, она существенно уменьшает производительность. 
        ///  Для сравнения, построение хеша для изображения 600х600px без медианной 
        ///  фильтрации занимает ~0.56с, а с ней - в районе ~5с (т.е. примерно в 10 раз больше времени).
        ///  Учитывая вышесказанное, я не рекомендую использовать данный флаг.
        /// </summary>
        public bool IsNeedMedianFilter { get; set; }
        /// <summary>
        /// Статус создания хеша
        /// </summary>
        public CreateHashStatuses Status { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public CreateHashTask()
        {
            //Инициализируем дефолтные значения
            Path = FileName = null;
            IsNeedMedianFilter = false;
            Hash = null;
            Status = CreateHashStatuses.Added;
        }

        /// <summary>
        /// Получаем флаг завершения работы над хешем
        /// </summary>
        /// <returns>True - работа была завершена</returns>
        public bool IsCompleteStatus() => (
            (Status == CreateHashStatuses.Complete) ||
            (Status == CreateHashStatuses.Error)
        );

        /// <summary>
        /// Получаем флаг ожидания запуска работы над хешем
        /// </summary>
        /// <returns>True - задача ждёт начала выполнения</returns>
        public bool IsWaitStatus() =>
            (Status == CreateHashStatuses.Added);

        /// <summary>
        /// Получаем флаг работы над хешем
        /// </summary>
        /// <returns>True - задача выполняется</returns>
        public bool IsWorkStatus() =>
            (Status == CreateHashStatuses.Added);

        /// <summary>
        /// Побновляем статус по наличию хеша
        /// </summary>
        public void UpdateStatusByHash() =>
            Status = (Hash.HasValue) 
                ? CreateHashStatuses.Complete 
                : CreateHashStatuses.Error;

        /// <summary>
        /// Получаем полный путь к файлу
        /// </summary>
        /// <returns>Строка полного пути к файлу</returns>
        public string GetFullPath() =>
            $"{GetPath()}{FileName}";

        /// <summary>
        /// Получваем путь со слешем
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            StringBuilder sb = new StringBuilder();
            //Если есть путь удаления
            if (Path != null)
            {
                //Добавляем в строку путь загрузки
                sb.Append(Path);
                //Если путь не оканчивается слешем
                if (Path.Last() != '\\')
                    //Добавляем в строку слеш
                    sb.Append("\\");
            }
            return sb.ToString();
        }
    }
}
