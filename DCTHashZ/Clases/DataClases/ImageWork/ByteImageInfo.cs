﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.DataClases.ImageWork
{
    /// <summary>
    /// Класс, хранящий данные об изображении
    /// </summary>
    class ByteImageInfo
    {
        /// <summary>
        /// Пиксели изображения
        /// </summary>
        public byte[] Pixels { get; set; }
        /// <summary>
        /// Размер изображения
        /// </summary>
        public Size ImageSize { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ByteImageInfo()
        {
            //Проставляем дефолтные значения
            Pixels = null;
            ImageSize = new Size();
        }

    }
}
