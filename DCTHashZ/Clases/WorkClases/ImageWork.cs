using DCTHashZ.Clases.DataClases;
using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZ.Clases.DataClases.Other;
using DCTHashZ.Clases.WorkClases.Filters;
using DCTHashZ.Clases.WorkClases.HashWork;
using DCTHashZ.Clases.WorkClases.Loader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DCTHashZ.Clases.DataClases.Other.Enums;

namespace DCTHashZ.Clases.WorkClases
{
    /// <summary>
    /// Класс превращения изображения в хеш
    /// </summary>
    internal class ImageWork
    {
        /// <summary>
        /// Флаг выполнения работы 
        /// </summary>
        public bool IsWork { get; set; }

        /// <summary>
        /// Класс загрузки изображения
        /// </summary>
        private LoadImagePixels loader;
        /// <summary>
        /// Класс перевода изображения в режим градаций серого
        /// </summary>
        private GrayScaleTransform grayScaleTransform;
        /// <summary>
        /// Класс уменьшения изображения
        /// </summary>
        private DecreaseImage decreaseImage;
        /// <summary>
        /// Класс медианной фильтрации изображения
        /// </summary>
        private MedianFilter medianFilter;
        /// <summary>
        /// Класс ДКП-фильтра изображения
        /// </summary>
        private DctFilter dctFilter;
        /// <summary>
        /// Класс рассчёта ДКП-хеша
        /// </summary>
        private CalculateDctHash calculateDctHash;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImageWork()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Проставляем дефолтные значения
            IsWork = false;
            //Инициализируем используемые классы
            loader = new LoadImagePixels();
            grayScaleTransform = new GrayScaleTransform();
            medianFilter = new MedianFilter();
            decreaseImage = new DecreaseImage();
            dctFilter = new DctFilter(Constants.DECREASED_IMAGE_SIZE);
            calculateDctHash = new CalculateDctHash(Constants.DCT_MATRIX_MAIN_BLOCK_SIZE);
        }

        /// <summary>
        /// Формируем ДКП-матрицу изображения
        /// </summary>
        /// <param name="taskInfo">Информация о выполняемой задаче</param>
        /// <returns>ДКП-матрица изображения</returns>
        private double[,] CreateDctMatrix(CreateHashTask taskInfo)
        {
            //Получаем пиксели изображения
            ByteImageInfo image = loader.LoadImage(taskInfo.GetFullPath());
            //Конвертируем изображение в режим градаций серого
            grayScaleTransform.ToGrayScale(ref image);
            //Если нужно применить медианный фильтр для сглаживания шумов на изображении
            if (taskInfo.IsNeedMedianFilter)
                //Применяем медианный филЬтр, для уменьшения уровня шума
                medianFilter.Filter(ref image, Constants.MEDIAN_FILTER_VALUE);
            //Уменьшаем изображение до уровня 32х32
            decreaseImage.SupersamplingDecrease(
                ref image,
                new Size(
                    Constants.DECREASED_IMAGE_SIZE,
                    Constants.DECREASED_IMAGE_SIZE
                ));
            //Высчитываем ДКП-матрицу из пикселей изображения
            return dctFilter.CalculateDCTValuesTable(image);
        }



        /// <summary>
        /// Рассчитываем хеш для изображения
        /// </summary>
        /// <param name="taskInfo">Информация о выполняемой задаче</param>
        public void CalculateHash(CreateHashTask taskInfo)
        {
            
            //Указываем что работа началась
            IsWork = true;
            //Запускаем эту работу в отдельном потоке
            new Thread(() => { 
                //Высчитываем ДКП-матрицу из изображения
                double[,] dctMatrix = CreateDctMatrix(taskInfo);
                //Если матрица не была получена
                if (dctMatrix == null)
                    //Проставляем статус ошибки
                    taskInfo.Status = CreateHashStatuses.Error;
                //Если всё ок
                else
                {
                    //Формируем хеш изображения и возвращаем его
                    taskInfo.Hash = calculateDctHash.CalculateHash(dctMatrix);
                    //Обновляем статус по наличию хеша
                    taskInfo.UpdateStatusByHash();
                }
                //Принудительно вызываем сборщик мусора
                //По какой-то причине он не хочет вызываться 
                //автоматически, по завершению работы с данным методом
                GC.Collect();
                //Указываем что работа завершена
                IsWork = false;
            }).Start();
        }

    }
}
