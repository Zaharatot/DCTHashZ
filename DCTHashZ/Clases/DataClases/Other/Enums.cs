using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.DataClases.Other
{
    /// <summary>
    /// Класс глобальных перечислений
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Перевчичление статусов создания хешей
        /// </summary>
        public enum CreateHashStatuses
        {
            /// <summary>
            /// Файл добавлен для создания хеша
            /// </summary>
            Added,
            /// <summary>
            /// Хеш создаётся
            /// </summary>
            Creation,
            /// <summary>
            /// Создание хеша завершено
            /// </summary>
            Complete,
            /// <summary>
            /// ОШибка генерации хеша
            /// </summary>
            Error
        }

    }
}
