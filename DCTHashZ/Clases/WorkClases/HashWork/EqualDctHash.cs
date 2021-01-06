using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCTHashZ.Clases.WorkClases.HashWork
{
    /// <summary>
    /// Класс сравнения хешей
    /// </summary>
    internal class EqualDctHash
    {
        /// <summary>
        /// Чувствительность сравнения по хешу
        /// </summary>
        private int searchSensivity;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="searchSensivity">Чувствительность сравнения по хешу</param>
        public EqualDctHash(int searchSensivity)
        {
            //Проставляем переданные значения
            this.searchSensivity = searchSensivity;
        }


        /// <summary>
        /// Получаем схожесть хешей
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>Значение схожести хешей</returns>
        public int GetHashSimilarity(ulong first, ulong second)
        {
            int ex = 0;
            //Проходимся по битам
            for (int i = 63; i >= 0; i--)
            {
                //Получаем биты и сравниваем
                if ((first >> i & 1) != (second >> i & 1))
                    //Если биты не равны - увеличиваем выход
                    ex++;
            }
            return ex;
        }

        /// <summary>
        /// Сравниваем хеши изображений
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>True - картинки схожы</returns>
        public bool EqalImageHash(ulong first, ulong second) =>            
            //В зависимости от разности хешей возвращаем результат проверки
            (GetHashSimilarity(first, second) < searchSensivity);


    }
}
