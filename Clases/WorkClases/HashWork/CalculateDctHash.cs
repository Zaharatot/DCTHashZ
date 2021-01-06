using DCTHashZ.Clases.WorkClases.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.WorkClases.HashWork
{
    /// <summary>
    /// Класс вычисления DCT-хеша
    /// </summary>
    internal class CalculateDctHash
    {
        /// <summary>
        /// Размер основного блока матрицы
        /// </summary>
        private int mainBlockSize;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="mainBlockSize">Размер основного блока матрицы</param>
        public CalculateDctHash(int mainBlockSize)
        {
            //Проставляем переданные значения
            this.mainBlockSize = mainBlockSize;
        }


        /// <summary>
        /// РАссчитываем хеш из основного блока матрицы дкп
        /// </summary>
        /// <param name="mainBlock">Основной блок матрицы ДКП</param>
        /// <param name="average">Среднее значение основного блока</param>
        /// <returns>Хеш матрицы</returns>
        private ulong CalculateHash(List<double> mainBlock, double average) 
        {
            ulong ex = 0;
            int id = 0;
            //ПРоходимся по значениям блока
            foreach(var value in mainBlock)
            {
                //Прибавляем значение выбранное по элементу
                //основного блока матрицы к значению хеша
                ex += (ulong)((value > average) ? 1 : 0);
                //Если мы не дошли до конца значения
                if (id++ < 63)
                    //Сдвигаем хеш на один бит
                    ex <<= 1;
            }
            return ex;
        }

        /// <summary>
        /// ПОлучаем основную часть ДКП-матрицы
        /// </summary>
        /// <param name="dctMatrix">ДКП-матрица для вычисления хеша</param>
        /// <param name="mainBlockSize">Размер основного блока матрицы</param>
        /// <returns>Список элементов матрицы</returns>
        private List<double> GetMatrixMainPart(double[,] dctMatrix, int mainBlockSize)
        {
            List<double> ex = new List<double>();
            //Проходимся по элементам матрицы, добавляя их в список
            for (int i = 0; i < mainBlockSize; i++)
                for (int j = 0; j < mainBlockSize; j++)
                    ex.Add(dctMatrix[i, j]);
            return ex;
        }


        /// <summary>
        /// Метод вычисления ДКП-хеша из матрицы
        /// </summary>
        /// <param name="dctMatrix">ДКП-матрица для вычисления хеша</param>
        /// <returns>Хеш матрицы</returns>
        public ulong? CalculateHash(double[,] dctMatrix)
        {
            ulong? ex = null;
            //Если матрица существует
            if (dctMatrix != null)
            {
                //Получаем основной рабочий блок матрицы
                List<double> mainBlock = GetMatrixMainPart(dctMatrix, mainBlockSize);
                //Рассчитываем среднее значение для основного блока матрицы
                double average = mainBlock.Average();
                //Рассчитываем хеш для матрицы
                ex = CalculateHash(mainBlock, average);
            }
            return ex;
        }
    }
}
