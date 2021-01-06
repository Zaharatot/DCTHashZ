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
    /// Класс, выполняющий уменьшение изображения
    /// </summary>
    class DecreaseImage : BaseFilter
    {

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DecreaseImage()
        {

        }

        /// <summary>
        /// Получаем размеры одного блока уменьшения, который будет сжат в один пиксель
        /// </summary>
        /// <param name="newImageSize">Размеры нового изображения</param>
        /// <param name="oldImageSize">Размеры старого изображения</param>
        /// <returns>Размер одного блока</returns>
        private Size GetDecreaseBlockSize(Size newImageSize, Size oldImageSize)
        {
            //Получаем ширину и высоту блока
            int width = oldImageSize.Width / newImageSize.Width;
            int height = oldImageSize.Height / newImageSize.Height;
            //Если есть неполный блок, то добавляем по пикселю
            if (oldImageSize.Width % newImageSize.Width != 0)
                width++;
            if (oldImageSize.Height % newImageSize.Height != 0)
                height++;
            return new Size(width, height);
        }

        /// <summary>
        /// Получаем цвет пикселя
        /// </summary>
        /// <param name="id">Id текущего выбранного пикселя</param>
        /// <param name="decreaseBlockSize">Размер блока, который мы ужимаем в один пиксель</param>
        /// <param name="info">Информация об изображении</param>
        /// <param name="count">Количество пикселей в массиве</param>
        /// <returns>Цвет усреднённого пикселя</returns>
        private byte GetPixelColor(ByteImageInfo info, int count, Size decreaseBlockSize, int id)
        {
            //Считываем список пикселей блока
            List<byte> block = GetPixelsBlock(id, decreaseBlockSize, count, info);
            //Возвращаем среднее значение цвета
            return (byte)block.Average(pixel => pixel);
        }

        /// <summary>
        /// Получаем стартовую позицию блока
        /// </summary>
        /// <param name="decreaseBlockSize">Размер блока уменьшения</param>
        /// <param name="imageWidth">Ширина изображения</param>
        /// <returns>Стартовая позиция блока</returns>
        protected Point GetStartPosition(Size decreaseBlockSize, int imageWidth) =>
            new Point(
                //Сдвигаемся по оси X на половину ширины блока
                decreaseBlockSize.Width / 2,
                //Сдвигаемся по оси Y на половину высоты блока
                decreaseBlockSize.Height / 2
            );


        /// <summary>
        /// Уменьшаем изображение методом суперсемплинга
        /// </summary>
        /// <param name="newImageSize">Размер нового изображения</param>
        /// <param name="info">Информация об изображении</param>
        public void SupersamplingDecrease(ref ByteImageInfo info, Size newImageSize)
        {
            byte[] pixels;
            try
            {
                //Если пиксели получены
                if ((info != null) && (info.Pixels != null))
                {
                    //Идентификаторs позициq в новом и старом массиве
                    int newId = 0;
                    int oldId;
                    //Получаем новый размер изображения
                    int count = newImageSize.Width * newImageSize.Height;
                    //Получаем размер изображения
                    int countOldImage = info.ImageSize.Width * info.ImageSize.Height;
                    //Инициализируем выходной массив пикселей
                    pixels = new byte[count];
                    //Получаем размеры блока, который будет свёртываться в один пиксель
                    Size decreaseBlockSize = GetDecreaseBlockSize(newImageSize, info.ImageSize);
                    //Получаем стартовую позицию для блока
                    Point startPosition = 
                        GetStartPosition(decreaseBlockSize, info.ImageSize.Width);
                    //Проходимся по строкам основного изображения
                    for (int y = startPosition.Y; y < info.ImageSize.Height; y += decreaseBlockSize.Height)
                    {
                        //Проходимся по пикселям строки основного изображения 
                        for (int x = startPosition.X; x < info.ImageSize.Width; x += decreaseBlockSize.Width)
                        {
                            //ПОлучаем позицию в старом массиве
                            oldId = y * info.ImageSize.Width + x;
                            //Получаем пиксель для нового массива
                            pixels[newId++] = GetPixelColor(info,
                               countOldImage, decreaseBlockSize, oldId);
                        }
                    }
                    //Проставляем новый массив пикселей и размер
                    info.Pixels = pixels;
                    info.ImageSize = newImageSize;
                }
            }
            catch { info = null; }
        }
    }
}
