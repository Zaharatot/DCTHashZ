using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.DataClases.ImageWork
{
    /// <summary>
    /// Класс, описывающий один пиксель изображения
    /// </summary>
    public class ImagePixel
    {
        /// <summary>
        /// Альфа канал
        /// </summary>
        public byte Alpha { get; set; }
        /// <summary>
        /// Красный канал
        /// </summary>
        public byte Red { get; set; }
        /// <summary>
        /// Зелёный канал
        /// </summary>
        public byte Green { get; set; }
        /// <summary>
        /// Синий канал
        /// </summary>
        public byte Blue { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ImagePixel()
        {
            //Проставляем переданные значения
            Alpha = Red = Green = Blue = 0;
        }


    }
}
