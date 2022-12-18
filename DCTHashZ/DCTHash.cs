using DCTHashZ.Clases.DataClases;
using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZ.Clases.DataClases.Other;
using DCTHashZ.Clases.WorkClases;
using DCTHashZ.Clases.WorkClases.Filters;
using DCTHashZ.Clases.WorkClases.HashWork;
using DCTHashZ.Clases.WorkClases.Loader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DCTHashZ.Clases.DataClases.Other.Delegates;

namespace DCTHashZ
{
    /// <summary>
    /// Фасадный класс библиотеки
    /// </summary>
    public class DCTHash : IDisposable
    {
        /// <summary>
        /// Событие обновления статусов создания хешей
        /// </summary>
        public static event UpdateCreationStatusEventHandler UpdateCreationStatus;


        /// <summary>
        /// Основной рабочий класс
        /// </summary>
        private MainWork mainWork;
        /// <summary>
        /// Класс сравнения хешей
        /// </summary>
        private EqualDctHash equalHash;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public DCTHash()
        {
            //Инициализируем используемые классы
            mainWork = new MainWork();
            equalHash = new EqualDctHash(Constants.DCT_HASH_EQUAL_SENSIVITY);
        }

      
        /// <summary>
        /// Метод вызова события обновления статусов создания хешей
        /// </summary>
        /// <param name="waitCount">Количество ожидающих выполнения задач</param>
        internal static void InvokeUpdateCreationStatus(int waitCount) =>
            //Вызываем внешний ивент
            UpdateCreationStatus?.Invoke(waitCount);



        /// <summary>
        /// Добавляем задачи для генерации хешей
        /// </summary>
        /// <param name="isNeedMedianFilter">
        ///  Данный флаг активирует медианную фильтрацию, при выполнении упрощения изображения. 
        ///  ВНИМАНИЕ! Хоть медианная фильтрация и добавляет некоторый процент точности при 
        ///  итоговом сравнении хешей, она существенно уменьшает производительность. 
        ///  Для сравнения, построение хеша для изображения 600х600px, без медианной 
        ///  фильтрации занимает ~0.56с, а с ней - в районе ~5с (т.е. примерно в 10 раз больше времени).
        ///  Учитывая вышесказанное, я не рекомендую использовать данный флаг.
        /// </param>
        /// <param name="paths">Список путей к файлам изображений</param>
        /// <returns>Список задач по генерации хешей</returns>
        public List<Task<CreateHashTask>> AddTasksAsync(List<string> paths, bool isNeedMedianFilter = false) =>
            //Вызываем внутренний метод
            mainWork.AddTasksAsync(paths, isNeedMedianFilter);

        /// <summary>
        /// Добавляем задачу для генерации хешей по загруженной картинке
        /// </summary>
        /// <param name="isNeedMedianFilter">
        ///  Данный флаг активирует медианную фильтрацию, при выполнении упрощения изображения. 
        ///  ВНИМАНИЕ! Хоть медианная фильтрация и добавляет некоторый процент точности при 
        ///  итоговом сравнении хешей, она существенно уменьшает производительность. 
        ///  Для сравнения, построение хеша для изображения 600х600px, без медианной 
        ///  фильтрации занимает ~0.56с, а с ней - в районе ~5с (т.е. примерно в 10 раз больше времени).
        ///  Учитывая вышесказанное, я не рекомендую использовать данный флаг.
        /// </param>
        /// <param name="height">Высота изображения</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="pixels">Массив пикселей изображения в формате RGBA</param>
        /// <returns>Задача по генерации хешей</returns>
        public Task<CreateHashTask> AddTaskAsync(byte[] pixels, int width, int height, bool isNeedMedianFilter = false) =>
            //Вызываем внутренний метод
            mainWork.AddTaskAsync(new ByteImageInfo() { 
                ImageSize = new Size(width, height),
                Pixels = pixels
            }, isNeedMedianFilter);


        /// <summary>
        /// Получаем схожесть хешей
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>Значение схожести хешей</returns>
        public int GetHashSimilarity(ulong first, ulong second) =>
            //Вызываем внутренний метод
            equalHash.GetHashSimilarity(first, second);

        /// <summary>
        /// Сравниваем хеши изображений
        /// </summary>
        /// <param name="first">Первый хеш</param>
        /// <param name="second">Второй хеш</param>
        /// <returns>True - картинки схожы</returns>
        public bool EqalImageHash(ulong first, ulong second) =>
            //Вызываем внутренний метод
            equalHash.EqalImageHash(first, second);

        /// <summary>
        /// Метод очистки неуправляемых ресмурсов класса
        /// </summary>
        public void Dispose()
        {
            //Завершаем работу основного класса
            mainWork?.Dispose();
        }
    }
}
