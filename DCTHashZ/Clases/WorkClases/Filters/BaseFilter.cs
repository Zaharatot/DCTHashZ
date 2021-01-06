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
    /// Базовый класс фильтра
    /// </summary>
    internal class BaseFilter
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public BaseFilter()
        {

        }

        /// <summary>
        /// Рассчитываем минимальную позицию для блока
        /// </summary>
        /// <param name="id">Id текущего выбранного пикселя</param>
        /// <param name="halfWidth">Половина ширины блока</param>
        /// <param name="halfHeight">Половина высоты блока</param>
        /// <param name="imageSize">Размер изображения</param>
        /// <returns>Id первого пикселя блока</returns>
        private int CalculateMinId(int id, int halfWidth, int halfHeight, Size imageSize)
        {
            //Получаем позицию, на сколько строк вверх нужно будет уйти
            int minId = id - halfHeight * imageSize.Width;
            //Если мы ушли за пределы массива по оси Y
            if (minId < 0)
                //Мы переходим на такую же позицию по оси X,
                //для нулевой строки пикселей
                minId = id % imageSize.Width;
            //Сдвигаемся к началу блока
            minId -= halfWidth;
            //Если мы ушли за пределы массива по оси X
            if (minId < 0)
                //Переходим к нулевому пикселю массива
                minId = 0;
            //Возвращаем резлультат
            return minId;
        }

        /// <summary>
        /// Рассчитываем максимальную позицию для блока
        /// </summary>
        /// <param name="id">Id текущего выбранного пикселя</param>
        /// <param name="halfWidth">Половина ширины блока</param>
        /// <param name="halfHeight">Половина высоты блока</param>
        /// <param name="imageSize">Размер изображения</param>
        /// <param name="count">Количество пикселей в массиве</param>
        /// <returns>Id последнего пикселя блока</returns>
        private int CalculateMaxId(int id, int halfWidth, int halfHeight, int count, Size imageSize)
        {
            //Получаем позицию, на сколько строк вниз нужно будет уйти
            int minId = id + halfHeight * imageSize.Width;
            //Если мы ушли за пределы массива по оси Y
            if (minId > count)
                //Мы переходим на такую же позицию по оси X,
                //для последней строки пикселей
                minId = count - imageSize.Width + id % imageSize.Width;
            //Сдвигаемся к концу блока
            minId += halfWidth;
            //Если мы ушли за пределы массива по оси X
            if (minId > count)
                //Переходим к последнему пикселю массива
                minId = count;
            //Возвращаем резлультат
            return minId;
        }

        /// <summary>
        /// Получаем блок пикселей изображения
        /// </summary>
        /// <param name="info">Информация об изображении</param>
        /// <param name="id">Id текущего выбранного пикселя</param>
        /// <param name="blockSize">Размер блока пикселей</param>
        /// <param name="count">Количество пикселей в массиве</param>
        /// <returns>Список пикселей считанных из блока</returns>
        protected List<byte> GetPixelsBlock(int id, Size blockSize, int count, ByteImageInfo info)
        {
            //Инициализируем выходной массив
            List<byte> ex = new List<byte>();
            //Максимальное значение по оси x
            int maxX;
            //Получаем значения половин ширины и высоты
            int halfWidth = blockSize.Width / 2;
            int halfHeight = blockSize.Height / 2;
            //Рассчитываем минимальныую точку блока
            int minId = CalculateMinId(id, halfWidth, halfHeight, info.ImageSize);
            //Рассчитываем максимальную точку блока
            int maxId = CalculateMaxId(id, halfWidth, halfHeight, count, info.ImageSize);
            //Проходим по строкам массива пикселей
            for (int i = minId; i < maxId; i += info.ImageSize.Width)
            {
                //Получаем максимальную позицию для 
                //считывания пикселей из строки
                maxX = Math.Min(i + halfWidth * 2 + 1, count);
                //Проходимся по пикселям строки
                for (int j = i; j < maxX; j++)
                    //Считываем их в выходной список
                    ex.Add(info.Pixels[j]);
            }
            //Возвращзаем результат
            return ex;
        }


    }
}
