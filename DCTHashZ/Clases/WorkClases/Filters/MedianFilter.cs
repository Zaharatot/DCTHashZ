using DCTHashZ.Clases.DataClases;
using DCTHashZ.Clases.DataClases.ImageWork;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.WorkClases.Filters
{
    /// <summary>
    /// Класс выполнения медианной фильтрации
    /// </summary>
    internal class MedianFilter : BaseFilter
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MedianFilter()
        {

        }

       
        /// <summary>
        /// ПОлучаем центральное значение цвета пикселей блока
        /// </summary>
        /// <param name="sortedBlock">Отсортироованный список пикселей</param>
        /// <returns>Центральное значение цвета</returns>
        private byte GetCenterValue(List<byte> sortedBlock)
        {
            byte ex = 0;
            int id = sortedBlock.Count / 2;
            //Если число пикселей чётное
            if (sortedBlock.Count % 2 == 0)
                //ПОлучаем среднее значение от двух центральных пикселов
                ex = (byte)((sortedBlock[id] + sortedBlock[id - 1]) / 2);
            //Если число пикселей не чётное
            else
                //ПОлучаем значение центрального пикселя
                ex = sortedBlock[id];
            return ex;
        }

        /// <summary>
        /// Получаем цвет нового пикселя
        /// </summary>
        /// <param name="id">Id текущего выбранного пикселя</param>
        /// <param name="info">Информация об изображении</param>
        /// <param name="blockSize">Размер блока пикселей</param>
        /// <param name="count">Количество пикселей в массиве</param>
        /// <returns>Цвет нового пикселя</returns>
        private byte GetNewPixelColor(int id, Size blockSize, int count, ByteImageInfo info)
        {
            //Считываем список пикселей блока
            List<byte> block = GetPixelsBlock(id, blockSize, count, info);
            //Сортитруем блок пикселей по возрастанию
            List<byte> sortedBlock = block.OrderBy(pixel => pixel).ToList();
            //Возвращаем значение центрального цвета
            return GetCenterValue(sortedBlock);
        }

        /// <summary>
        /// Выполняем медианную фильтрацию
        /// </summary>
        /// <param name="value">Значение фильтрации</param>
        /// <param name="info">Информация об изображении</param>
        public void Filter(ref ByteImageInfo info, int value)
        {
            byte[] pixels;
            try
            {
                //Если пиксели получены
                if ((info != null) && (info.Pixels != null))
                {
                    //Инициализируем выходной массив пикселей
                    pixels = new byte[info.Pixels.Length];
                    //Получаем размеры блка, который будет использоваться для фильтрации
                    Size blockSize = new Size(value, value);

                    DateTime dt = DateTime.Now;

                    //Проходимся по пикселям массива
                    for (int i = 0; i < info.Pixels.Length; i++)
                        //Проставляем обновлённые пиксели
                        pixels[i] = GetNewPixelColor(i, blockSize,
                            info.Pixels.Length, info);

                    DateTime dt1 = DateTime.Now;

                    double result = (dt1 - dt).TotalSeconds;
                    //Проставляем новый массив пикселей
                    info.Pixels = pixels;
                }
            }
            catch { info = null; }
        }

    }
}
