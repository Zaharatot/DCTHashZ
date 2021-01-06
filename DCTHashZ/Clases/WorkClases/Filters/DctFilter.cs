using DCTHashZ.Clases.DataClases;
using DCTHashZ.Clases.DataClases.ImageWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.WorkClases.Filters
{
    /// <summary>
    /// Класс, выполняющих дискретное косинусное 
    /// преобразование изображения
    /// </summary>
    internal class DctFilter
    {
        

        /// <summary>
        /// Матрица ДКП
        /// </summary>
        private double[,] dctMatrix;
        /// <summary>
        /// Транспонированная матрица ДКП
        /// </summary>
        private double[,] dctMatrixTransp;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="size">Высота и ширина сжатого изображения</param>
        public DctFilter(int size)
        {
            //Вычисляем ДКП-матрицу, т.к. она константная
            dctMatrix = CalculateDCTMatrix(size);
            //Транспонируем ДКП-матрицу
            dctMatrixTransp = TranspositionMatrix(dctMatrix, size);
        }



        /// <summary>
        /// Транспонируем матрицу
        /// </summary>
        /// <param name="matrix">Исходная матрица</param>
        /// <param name="size">Высота и ширина квадратной матрицы</param>
        /// <returns>Транспонирвоанная матрица</returns>
        private double[,] TranspositionMatrix(double[,] matrix, int size)
        {
            //Иницйиализируем выходную матрицу
            double[,] ex = new double[size, size];
            //Проходимся по строкам
            for (int i = 0; i < size; i++)
                //Проставляем элементы, меняя 
                //местами строки и столбцы
                for (int j = 0; j < size; j++)
                    ex[i, j] = matrix[j, i];            
            return ex;
        }

        /// <summary>
        /// Рассчитываем ДКП-матрицу
        /// </summary>
        /// <param name="size">Высота и ширина квадратной матрицы</param>
        /// <returns>Матрица ДКП</returns>
        private double[,] CalculateDCTMatrix(int size)
        {
            //Иницйиализируем ДКП-матрицу
            double[,] ex = new double[size, size];
            //Рассчитываем значения, которые 
            //используются во многих вычислениях
            double oneSqrt = Math.Sqrt(1d / size);
            double twoSqrt = Math.Sqrt(2d / size);
            double doubleN = size / 2d;
            //Проходимся по строкам
            for (int i = 0; i < size; i++)
                //Проходимся по ячейкам матрицы
                for (int j = 0; j < size; j++)
                    //Проставляем вычесленные значения матрицы ДКП
                    ex[i, j] = (j == 0) ? oneSqrt 
                        : CalculateDCTMatrixElement(i, j, twoSqrt, doubleN);
            return ex;
        }


        /// <summary>
        /// Высчитываем значение для элемента ДКП-матрицы
        /// </summary>
        /// <param name="m">Значение m, для элемента матрицы</param>
        /// <param name="n">Значение n, для элемента матрицы</param>
        /// <param name="sqrt">Значение корня</param>
        /// <param name="doubleN">Значение уджвоенного N</param>
        /// <returns>Вычисленное значение</returns>
        private double CalculateDCTMatrixElement(int n, int m, double sqrt, double doubleN) =>
            sqrt * Math.Cos(((2d * n + 1d) * m * Math.PI) / doubleN);

        /// <summary>
        /// Перемножаем матрицы
        /// </summary>
        /// <param name="firstMatrix">Первая матрица для перемножения</param>
        /// <param name="secondMatrix">Вторая матрица для перемножения</param>
        /// <param name="size">Высота и ширина сжатого изображения</param>
        /// <returns>Матрица произведения</returns>
        private double[,] MulMatrix(double[,] firstMatrix, double[,] secondMatrix, int size)
        {
            double[,] ex = new double[size, size];
            //Перемножаем матрицу пикселей изображения
            //на матрицу ДКП, и возвращаем результат
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //Обнуляем элемент матрицы
                    ex[i, j] = 0;
                    //Проходимся по строке и столбцу матрицы
                    for (int k = 0; k < size; k++)
                        //Прибавляем к выходному значению результат
                        //перемножения элементов матриц
                        ex[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }
            return ex;
        }

        /// <summary>
        /// Конвертируем массив пикселей типа byte, в матрицу с типом double
        /// </summary>
        /// <param name="info">Информация об изображении</param>
        /// <returns>Массиф типа double</returns>
        private double[,] CastToDouble(ByteImageInfo info)
        {
            //Инициализируем выходной массив
            double[,] ex = new double[info.ImageSize.Width, info.ImageSize.Height];
            //Идентификатор для ммассива пикселей
            int id = 0;
            //Проходимся по строкам
            for (int i = 0; i < info.ImageSize.Height; i++)
                //Проставляем элементы массива в выходной
                for (int j = 0; j < info.ImageSize.Width; j++)
                    ex[i, j] = info.Pixels[id++];            
            return ex;
        }
        

        /// <summary>
        /// Высчитываем таблицу значений ДКП
        /// </summary>
        /// <param name="info">Информация об изображении</param>
        /// <returns>Массив вычисленных значений</returns>
        public double[,] CalculateDCTValuesTable(ByteImageInfo info)
        {
            double[,] ex = null;
            //Если пиксели получены
            if ((info != null) && (info.Pixels != null))
            {
                //Конвертируем массив пикселей в тип double
                double[,] doublePixelMatrix = CastToDouble(info);
                //Умножаем матрицу ДКП на матрицу пикселей
                ex = MulMatrix(dctMatrix, doublePixelMatrix, info.ImageSize.Width);
                //Умножаем матрицу пикселей на транспонированную матрицу ДКП
                ex = MulMatrix(ex, dctMatrixTransp, info.ImageSize.Width);
            }
            return ex;
        }

    }
}
