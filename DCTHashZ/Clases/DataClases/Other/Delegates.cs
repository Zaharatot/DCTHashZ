using DCTHashZ.Clases.DataClases.ImageWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.DataClases.Other
{
    /// <summary>
    /// Класс глобальных делегатов событий
    /// </summary>
    public class Delegates
    {

        /// <summary>
        /// Делегат события завершения генерации хешей
        /// </summary>
        /// <param name="taskList">Список завершённых задач</param>
        public delegate void CompleteCenerationEventHadnler(List<CreateHashTask> taskList);

        /// <summary>
        /// Делегат события обновления статусов загрузки
        /// </summary>
        /// <param name="waitCount">Количество ожидающих выполнения задач</param>
        public delegate void UpdateCreationStatusEventHandler(int waitCount);

    }
}
