using DCTHashZ.Clases.DataClases;
using DCTHashZ.Clases.DataClases.Global;
using DCTHashZ.Clases.DataClases.ImageWork;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.WorkClases.Loader
{
    /// <summary>
    /// Класс загрузки изображения в виде 
    /// одномерного массива пикселей
    /// </summary>
    internal class LoadImagePixels
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public LoadImagePixels()
        {

        }

        /// <summary>
        /// Получаем пиксели изображения в виде одномерного массива
        /// </summary>
        /// <param name="img">Исходное изображение</param>
        /// <returns>Массив пикселей</returns>
        private byte[] GetImagePixels(Bitmap img)
        {
            byte[] pixels = null;
            try
            {
                //Получаем размер считываемого массива
                int size = img.Width * img.Height * 4;
                //ИНициализируем массив для пикселей
                pixels = new byte[size]; 
                //Лочим биты картинки
                var bd = img.LockBits(
                    new Rectangle(0, 0, img.Width, img.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb);
                //Считываем пиксели изображения
                Marshal.Copy(bd.Scan0, pixels, 0, size);
                //Разблокируем пиксели изображения
                img.UnlockBits(bd);
            }
            catch { pixels = null; }
            return pixels;
        }


        /// <summary>
        /// Загружаем изображение
        /// </summary>
        /// <param name="path">Путь для загрузки изображения</param>
        /// <returns>Информация о пикселях загруженного изображения</returns>
        public ByteImageInfo LoadImage(string path)
        {
            ByteImageInfo ex = null;
            byte[] pixels = null;
            try
            {
                //Если файл изображения существует
                if (File.Exists(path))
                {
                    //Загружаем оригинальнку картинку
                    Bitmap originalImage = new Bitmap(path);
                    //Принудительно меняем ей разрешение в 800х600
                    using (Bitmap sourceImage = new Bitmap(originalImage,
                        new Size(Constants.LOAD_IMAGE_WIDTH, Constants.LOAD_IMAGE_HEIGHT)))
                    {
                        //Уничтожаем оригинальное изображение
                        originalImage.Dispose();
                        //Получаем одномерный массив пикселей изображения
                        pixels = GetImagePixels(sourceImage);
                        //Если пиксели были корректно загружены
                        if (pixels != null)
                            //Инициализируем выходное значение
                            ex = new ByteImageInfo() {
                                //Конвертируем пиксели из одномерного массива 
                                //каналов в класс информации о пикселе
                                Pixels = pixels,
                                ImageSize = new Size(
                                    sourceImage.Width,
                                    sourceImage.Height
                                )
                            };
                    }
                }
            }
            catch { ex = null; }
            return ex;
        }

    }
}
