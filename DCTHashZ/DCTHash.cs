using DCTHashZ.Clases.DataClases.Global;
using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZ.Clases.DataClases.Interfaces;
using DCTHashZ.Clases.WorkClases;
using DCTHashZ.Clases.WorkClases.HashWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DCTHashZ.Clases.DataClases.Global.Delegates;

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
        private readonly MainWork mainWork;
        /// <summary>
        /// Класс сравнения хешей
        /// </summary>
        private readonly EqualDctHash equalHash;

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
        /// Рассчитываем хеш изображения по пути к файлу
        /// </summary>
        /// <param name="path">Путь к файлу на диске</param>
        /// <param name="isNeedMedianFilter">
        ///  Данный флаг активирует медианную фильтрацию, при выполнении упрощения изображения.
        ///  ВНИМАНИЕ! Хоть медианная фильтрация и добавляет некоторый процент точности при
        ///  итоговом сравнении хешей, она существенно уменьшает производительность.
        ///  Для сравнения, построение хеша для изображения 600х600px, без медианной
        ///  фильтрации занимает ~0.56с, а с ней - в районе ~5с (т.е. примерно в 10 раз больше времени).
        ///  Учитывая вышесказанное, я не рекомендую использовать данный флаг.
        /// </param>
        /// <returns>Результат генерации хеша</returns>
        public Task<CreateHashTask> CalculateHashAsync(string path, bool isNeedMedianFilter = false) =>
            mainWork.CalculateHashAsync(path, isNeedMedianFilter);

        /// <summary>
        /// Рассчитываем хеш загруженного изображения
        /// </summary>
        /// <param name="info">Класс информации об изображении, наследуемый от интерфейса</param>
        /// <param name="isNeedMedianFilter">
        ///  Данный флаг активирует медианную фильтрацию, при выполнении упрощения изображения.
        ///  ВНИМАНИЕ! Хоть медианная фильтрация и добавляет некоторый процент точности при
        ///  итоговом сравнении хешей, она существенно уменьшает производительность.
        ///  Для сравнения, построение хеша для изображения 600х600px, без медианной
        ///  фильтрации занимает ~0.56с, а с ней - в районе ~5с (т.е. примерно в 10 раз больше времени).
        ///  Учитывая вышесказанное, я не рекомендую использовать данный флаг.
        /// </param>
        /// <returns>Результат генерации хеша</returns>
        public Task<CreateHashTask> CalculateHashAsync(IImageInfo info, bool isNeedMedianFilter = false) =>
            mainWork.CalculateHashAsync(info, isNeedMedianFilter);

        /// <summary>
        /// Рассчитываем хеши для набора файлов
        /// </summary>
        /// <param name="paths">Список путей к файлам изображений</param>
        /// <param name="isNeedMedianFilter">
        ///  Данный флаг активирует медианную фильтрацию, при выполнении упрощения изображения.
        ///  ВНИМАНИЕ! Хоть медианная фильтрация и добавляет некоторый процент точности при
        ///  итоговом сравнении хешей, она существенно уменьшает производительность.
        ///  Для сравнения, построение хеша для изображения 600х600px, без медианной
        ///  фильтрации занимает ~0.56с, а с ней - в районе ~5с (т.е. примерно в 10 раз больше времени).
        ///  Учитывая вышесказанное, я не рекомендую использовать данный флаг.
        /// </param>
        /// <returns>Список результатов генерации хешей</returns>
        public Task<List<CreateHashTask>> CalculateHashesAsync(IEnumerable<string> paths, bool isNeedMedianFilter = false) =>
            mainWork.CalculateHashesAsync(paths, isNeedMedianFilter);

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
            mainWork.Dispose();
        }
    }
}
