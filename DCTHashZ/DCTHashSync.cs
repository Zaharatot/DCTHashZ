using DCTHashZ.Clases.DataClases;
using DCTHashZ.Clases.DataClases.Global;
using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZ.Clases.DataClases.Interfaces;
using DCTHashZ.Clases.WorkClases;
using DCTHashZ.Clases.WorkClases.Filters;
using DCTHashZ.Clases.WorkClases.HashWork;
using DCTHashZ.Clases.WorkClases.Loader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ
{
    /// <summary>
    /// Фасадный класс библиотеки, для синхронных операций
    /// </summary>
    public class DCTHashSync
    {

        /// <summary>
        /// Класс сравнения хешей
        /// </summary>
        private EqualDctHash equalHash;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DCTHashSync()
        {
            //Инициализируем используемые классы
            equalHash = new EqualDctHash(Constants.DCT_HASH_EQUAL_SENSIVITY);
        }


        /// <summary>
        /// Формируем хеш файла
        /// </summary>
        /// <param name="path">ПУть к файлу на диске</param>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <returns>Результат генерации хеша</returns>
        public CreateHashTask CalculateHash(string path, bool isNeedMedianFilter)
        {
            //ПОлучаем информацию о файле
            FileInfo fi = new FileInfo(path);
            //Инициализируем информацию о задаче
            CreateHashTask task = new CreateHashTask()
            {
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
        /// Получаем схожесть хешей
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>Значение схожести хешей</returns>
        public int GetHashSimilarity(ulong first, ulong second) =>
            //Вызываем внутренний метод
            equalHash.GetHashSimilarity(first, second);

        /// <summary>
        /// Сравниваем хеши изображений
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>True - картинки схожы</returns>
        public bool EqalImageHash(ulong first, ulong second) =>
            //Вызываем внутренний метод
            equalHash.EqalImageHash(first, second);

    }
}
