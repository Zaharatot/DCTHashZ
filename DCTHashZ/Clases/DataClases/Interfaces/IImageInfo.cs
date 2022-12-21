using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.DataClases.Interfaces
{
    /// <summary>
    /// Интерфейс класса информации об изображении
    /// </summary>
    public interface IImageInfo
    {
        /// <summary>
        /// Пиксели изображения
        /// </summary>
        byte[] Pixels { get; set; }
        /// <summary>
        /// Размер изображения
        /// </summary>
        Size ImageSize { get; set; }
        /// <summary>
        /// Флаг изображеняи в градациях серого
        /// </summary>
        bool IsGrayScale { get; set; }
    }
}
