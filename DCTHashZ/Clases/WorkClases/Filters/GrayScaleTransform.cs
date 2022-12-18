using DCTHashZ.Clases.DataClases;
using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZ.Clases.DataClases.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.WorkClases.Filters
{
    /// <summary>
    /// Класс фильтра предобразования пикселей картинки в 
    /// режим оттенков серого
    /// </summary>
    internal class GrayScaleTransform : BaseFilter
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public GrayScaleTransform()
        {

        }


        /// <summary>
        /// Возвращаем цвет пикселя, переведённый в градации серого
        /// </summary>
        /// <param name="red">Значение Красного канала</param>
        /// <param name="green">Значение зелёного канала</param>
        /// <param name="blue">Значение синего канала</param>
        /// <returns>Яркость пикселя в градациях серого</returns>
        private byte GetGrayScalePixel(byte red, byte green, byte blue) =>
            (byte)(0.299 * red + 0.587 * green + 0.114 * blue);


        /// <summary>
        /// Переводим картинку в режим градаций серого
        /// </summary>
        /// <param name="info">Класс информации об изображении, наследуемый от интерфейса</param>
        public void ToGrayScale(ref IImageInfo info)
        {
            byte[] pixels;
            try
            {
                //Если пиксели получены
                if ((info != null) && (info.Pixels != null))
                {
                    //Получаем итоговый размер выходного массива
                    int size = info.ImageSize.Width * info.ImageSize.Height;
                    //Идентификатор пикселя в выходном массиве
                    int id = 0;
                    //Инициализируем выходной массив
                    pixels = new byte[size];
                    //Проходимся по каналам
                    for (int i = 0; i < info.Pixels.Length; i += 4)
                        //Проставляем пиксели
                        //Альфа-канал мы игнорируем           
                        pixels[id++] = GetGrayScalePixel(
                            //Красный канал
                            info.Pixels[i + 1],
                            //Синий канал
                            info.Pixels[i + 2],
                            //Зелёный канал
                            info.Pixels[i + 3]
                        );           
                    //Проставляем новый массив пикселей
                    info.Pixels = pixels;
                }
            }
            catch { info = null; }
        }

    }
}
