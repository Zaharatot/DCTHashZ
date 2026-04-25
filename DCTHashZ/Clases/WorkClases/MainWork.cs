using DCTHashZ.Clases.DataClases.Global;
using DCTHashZ.Clases.DataClases.ImageWork;
using DCTHashZ.Clases.DataClases.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static DCTHashZ.Clases.DataClases.Global.Enums;

namespace DCTHashZ.Clases.WorkClases
{
    /// <summary>
    /// Основной рабочий класс
    /// </summary>
    internal class MainWork : IDisposable
    {
        /// <summary>
        /// Ограничитель параллельного вычисления хешей
        /// </summary>
        private readonly SemaphoreSlim executionLimiter;
        /// <summary>
        /// Количество задач в очереди и работе
        /// </summary>
        private int pendingTasksCount;
        /// <summary>
        /// Флаг очистки ресурсов
        /// </summary>
        private bool isDisposed;


        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MainWork()
        {
            executionLimiter = new SemaphoreSlim(
                Constants.SUNCHRONOUS_TASKS_COUNT,
                Constants.SUNCHRONOUS_TASKS_COUNT);
        }

        /// <summary>
        /// Формируем хеш файла
        /// </summary>
        /// <param name="path">ПУть к файлу на диске</param>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <returns>Результат генерации хеша</returns>
        private static CreateHashTask CalculateHash(string path, bool isNeedMedianFilter)
        {
            //ПОлучаем информацию о файле
            FileInfo fi = new FileInfo(path);
            //Инициализируем информацию о задаче
            CreateHashTask task = new CreateHashTask()
            {
                FileName = fi.Name,
                Path = fi.DirectoryName,
                IsNeedMedianFilter = isNeedMedianFilter,
                Status = CreateHashStatuses.Creation
            };
            //Инициализируем класс работы с изображением
            ImageWork imageWork = new ImageWork();
            //Рассчитываем хеш файла
            imageWork.CalculateHash(task);
            //Возвращаем результат
            return task;
        }

        /// <summary>
        /// Формируем хеш файла
        /// </summary>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <param name="info">Класс информации об изображении, наследуемый от интерфейса</param>
        /// <returns>Результат генерации хеша</returns>
        private static CreateHashTask CalculateHash(IImageInfo info, bool isNeedMedianFilter)
        {
            //Инициализируем информацию о задаче
            CreateHashTask task = new CreateHashTask()
            {
                IsNeedMedianFilter = isNeedMedianFilter,
                Status = CreateHashStatuses.Creation
            };
            //Инициализируем класс работы с изображением
            ImageWork imageWork = new ImageWork();
            //Рассчитываем хеш файла
            imageWork.CalculateHash(task, info);
            //Возвращаем результат
            return task;
        }

        /// <summary>
        /// Выполняем расчёт хеша с ограничением параллелизма
        /// </summary>
        /// <param name="hashCalculator">Делегат вычисления хеша</param>
        /// <returns>Результат генерации хеша</returns>
        private async Task<CreateHashTask> RunCalculationAsync(Func<CreateHashTask> hashCalculator)
        {
            ThrowIfDisposed();
            UpdatePendingTasksCount(1);
            await executionLimiter.WaitAsync().ConfigureAwait(false);
            try
            {
                return await Task.Run(hashCalculator).ConfigureAwait(false);
            }
            finally
            {
                executionLimiter.Release();
                UpdatePendingTasksCount(-1);
            }
        }

        /// <summary>
        /// Формируем хеш файла
        /// </summary>
        /// <param name="path">ПУть к файлу на диске</param>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <returns>Результат генерации хеша</returns>
        public Task<CreateHashTask> CalculateHashAsync(string path, bool isNeedMedianFilter) =>
            RunCalculationAsync(() => CalculateHash(path, isNeedMedianFilter));

        /// <summary>
        /// Формируем хеш изображения
        /// </summary>
        /// <param name="info">Класс информации об изображении, наследуемый от интерфейса</param>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <returns>Результат генерации хеша</returns>
        public Task<CreateHashTask> CalculateHashAsync(IImageInfo info, bool isNeedMedianFilter) =>
            RunCalculationAsync(() => CalculateHash(info, isNeedMedianFilter));

        /// <summary>
        /// Формируем хеши для набора файлов
        /// </summary>
        /// <param name="pathList">Список путей к файлам изображений</param>
        /// <param name="isNeedMedianFilter">Флаг необходимости использования медианного фильтра</param>
        /// <returns>Список результатов генерации хешей</returns>
        public async Task<List<CreateHashTask>> CalculateHashesAsync(IEnumerable<string> pathList, bool isNeedMedianFilter)
        {
            if (pathList == null)
                throw new ArgumentNullException(nameof(pathList));

            CreateHashTask[] result = await Task
                .WhenAll(pathList.Select(path => CalculateHashAsync(path, isNeedMedianFilter)))
                .ConfigureAwait(false);

            return result.ToList();
        }

        /// <summary>
        /// Обновляем количество ожидающих и выполняемых задач
        /// </summary>
        /// <param name="delta">Сдвиг значения счётчика</param>
        private void UpdatePendingTasksCount(int delta) =>
            DCTHash.InvokeUpdateCreationStatus(Interlocked.Add(ref pendingTasksCount, delta));

        /// <summary>
        /// Проверяем, что объект ещё не был очищен
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(MainWork));
        }

        /// <summary>
        /// Метод очистки неуправляемых ресмурсов класса
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;
            executionLimiter.Dispose();
        }
    }
}
