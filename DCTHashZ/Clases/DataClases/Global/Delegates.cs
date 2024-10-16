using DCTHashZ.Clases.DataClases.ImageWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.DataClases.Global
{
    /// <summary>
    /// Класс глобальных делегатов событий
    /// </summary>
    public class Delegates
    {
        /// <summary>
        /// Делегат события обновления статусов загрузки
        /// </summary>
        /// <param name="waitCount">Количество ожидающих выполнения задач</param>
        public delegate void UpdateCreationStatusEventHandler(int waitCount);

    }
}
