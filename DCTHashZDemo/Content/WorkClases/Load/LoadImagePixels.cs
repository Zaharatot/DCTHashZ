using DCTHashZDemo.Content.DataClases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZDemo.Content.WorkClases.Load
{
    /// <summary>
    /// Класс загрузки изображения в виде 
    /// одномерного массива пикселей
    /// </summary>
    internal class LoadImagePixels
    {
        /// <summary>
        /// Принудительная ширина загружаемого изображения
        /// </summary>
        private const int LOAD_IMAGE_WIDTH = 600;
        /// <summary>
        /// Принудительная высота загружаемого изображения
        /// </summary>
        private const int LOAD_IMAGE_HEIGHT = 800;

        /// <summary>
        /// Класс конвертации каналов в монохромные пиксели
        /// </summary>
        private GrayScaleTransform _grayScaleTransform;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public LoadImagePixels()
        {
            Init();
        }

        /// <summary>
        /// Инициализатор класса
        /// </summary>
        private void Init()
        {
            //Инициализируем используемые классы
            _grayScaleTransform = new GrayScaleTransform();
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
            byte[] bytes = null;
            try
            {
                //Если файл изображения существует
                if (File.Exists(path))
                {
                    //Загружаем оригинальнку картинку
                    Bitmap originalImage = new Bitmap(path);
                    //Принудительно меняем ей разрешение в 800х600
                    using (Bitmap sourceImage = new Bitmap(originalImage,
                        new Size(LOAD_IMAGE_WIDTH, LOAD_IMAGE_HEIGHT)))
                    {
                        //Уничтожаем оригинальное изображение
                        originalImage.Dispose();
                        //Получаем одномерный массив каналов изображения
                        bytes = GetImagePixels(sourceImage);
                        //Если каналы были корректно загружены
                        if (bytes != null)
                        {
                            //Получаем монохромные пиксели изображения
                            bytes = _grayScaleTransform.ToGrayScale(bytes);
                            //Инициализируем выходное значение
                            ex = new ByteImageInfo(sourceImage.Size, bytes);
                        }
                    }
                }
            }
            catch { ex = null; }
            return ex;
        }

    }
}
