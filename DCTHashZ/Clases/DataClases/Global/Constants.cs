using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.DataClases.Global
{
    /// <summary>
    /// Класс, включающий в себя все глобальные значения 
    /// для выполнения работы данного класса, в виде констант
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// Принудительная ширина загружаемого изображения
        /// </summary>
        public const int LOAD_IMAGE_WIDTH = 600;
        /// <summary>
        /// Принудительная высота загружаемого изображения
        /// </summary>
        public const int LOAD_IMAGE_HEIGHT = 800;
        /// <summary>
        /// Ширина и высота изображения (оно будет квадратным), 
        /// после его уменьшения, для последующего 
        /// косинусного преобразования 
        /// </summary>
        public const int DECREASED_IMAGE_SIZE = 32;
        /// <summary>
        /// Значение для применения медианного фильтра
        /// </summary>
        public const int MEDIAN_FILTER_VALUE = 8;
        /// <summary>
        /// Ширина и высота основного блока матрицы, 
        /// который будет использоваться для рассчёта хеша
        /// </summary>
        public const int DCT_MATRIX_MAIN_BLOCK_SIZE = 8;
        /// <summary>
        /// Чувствительность для сравнения изображений
        /// Дефолтом считается значение в 21, так что его и поставлю
        /// </summary>
        public const int DCT_HASH_EQUAL_SENSIVITY = 21;

        /// <summary>
        /// Время ожидания таймера обновления статусов задач
        /// Указывается в милисекундах
        /// </summary>
        public const int UPDATE_TASK_STATUSES_DELAY = 150;
        /// <summary>
        /// Время ожидания таймера получения задач
        /// Указывается в милисекундах
        /// </summary>
        public const int WAIT_TASKS_DELAY = 1000;
        /// <summary>
        /// Количество одновременно вычисляемых хешей
        /// </summary>
        public const int SUNCHRONOUS_TASKS_COUNT = 5;
    }
}
