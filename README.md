# DCTHashZ

#### EN description:

This library implements perceptual image comparison for C# applications.
The project was created as an open alternative to pHash-based libraries with restrictive licensing for commercial use. DCTHashZ is distributed under Apache 2.0.

Comparison is based on Discrete Cosine Transform Based Hash, also known as pHash. The implementation was assembled from public materials, including [this Habr article](https://habr.com/ru/post/237307/), Wikipedia, and practical comparison against the [demo pHash](https://www.phash.org/demo/).

The main entry point is the `DCTHash` facade class. The library now uses an asynchronous API:

- `CalculateHashAsync(string path, bool isNeedMedianFilter = false)` calculates the hash for an image file.
- `CalculateHashAsync(IImageInfo info, bool isNeedMedianFilter = false)` calculates the hash for a preloaded image.
- `CalculateHashesAsync(IEnumerable<string> paths, bool isNeedMedianFilter = false)` calculates hashes for a set of files.
- `GetHashSimilarity(ulong first, ulong second)` returns the similarity score.
- `EqalImageHash(ulong first, ulong second)` checks whether two hashes are considered equal.

To monitor progress you can subscribe to the `UpdateCreationStatus` event. It returns the current number of hash-generation operations that are still pending or in progress.

Example for file paths:

```csharp
using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using System.Collections.Generic;
using System.Threading.Tasks;

public async Task<List<CreateHashTask>> BuildHashesAsync(IEnumerable<string> paths)
{
    using (var hash = new DCTHash())
    {
        return await hash.CalculateHashesAsync(paths);
    }
}
```

Example for a single file:

```csharp
using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using System.Threading.Tasks;

public async Task<CreateHashTask> BuildHashAsync(string path)
{
    using (var hash = new DCTHash())
    {
        return await hash.CalculateHashAsync(path);
    }
}
```

Example for comparing two hashes:

```csharp
using DCTHashZ;

public bool IsSameImage(ulong first, ulong second)
{
    using (var hash = new DCTHash())
    {
        return hash.EqalImageHash(first, second);
    }
}
```

The demo project `DCTHashZDemo` was also updated to the async API and contains examples for:

- batch hash generation from file paths;
- hash comparison between images;
- hash generation from preloaded image data.

Compiled outputs for the library and the demo can be found in the [Compilation](https://github.com/Zaharatot/DCTHashZ/tree/main/Compilation) folder.


#### RU Description/Описание на русском:

Данная библиотека реализует перцептивное сравнение изображений для приложений на C#.
Проект был создан как открытая альтернатива pHash-реализациям с неудобными ограничениями по лицензированию в коммерческом использовании. DCTHashZ распространяется по лицензии Apache 2.0.

Сравнение реализовано через генерацию хеша методом дискретного косинусного преобразования, то есть pHash. В основе реализации лежат открытые материалы, включая [эту статью на Хабре](https://habr.com/ru/post/237307/), Wikipedia и сверку результатов с [demo pHash](https://www.phash.org/demo/).

Основная точка входа в библиотеку — фасадный класс `DCTHash`. Сейчас библиотека использует асинхронный API:

- `CalculateHashAsync(string path, bool isNeedMedianFilter = false)` вычисляет хеш изображения по пути к файлу.
- `CalculateHashAsync(IImageInfo info, bool isNeedMedianFilter = false)` вычисляет хеш для уже загруженного изображения.
- `CalculateHashesAsync(IEnumerable<string> paths, bool isNeedMedianFilter = false)` вычисляет хеши для набора файлов.
- `GetHashSimilarity(ulong first, ulong second)` возвращает степень сходства двух хешей.
- `EqalImageHash(ulong first, ulong second)` проверяет, считаются ли два хеша одинаковыми.

Для отслеживания прогресса можно подписаться на событие `UpdateCreationStatus`. Оно возвращает текущее количество операций генерации хеша, которые ещё ожидают выполнения или находятся в работе.

Пример расчёта хешей для списка файлов:

```csharp
using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using System.Collections.Generic;
using System.Threading.Tasks;

public async Task<List<CreateHashTask>> BuildHashesAsync(IEnumerable<string> paths)
{
    using (var hash = new DCTHash())
    {
        return await hash.CalculateHashesAsync(paths);
    }
}
```

Пример расчёта хеша для одного файла:

```csharp
using DCTHashZ;
using DCTHashZ.Clases.DataClases.ImageWork;
using System.Threading.Tasks;

public async Task<CreateHashTask> BuildHashAsync(string path)
{
    using (var hash = new DCTHash())
    {
        return await hash.CalculateHashAsync(path);
    }
}
```

Пример сравнения двух хешей:

```csharp
using DCTHashZ;

public bool IsSameImage(ulong first, ulong second)
{
    using (var hash = new DCTHash())
    {
        return hash.EqalImageHash(first, second);
    }
}
```

Демо-проект `DCTHashZDemo` также обновлён под новый async API и содержит примеры:

- пакетной генерации хешей по путям к файлам;
- сравнения изображений по хешам;
- генерации хеша из заранее загруженных данных изображения.

Собранные библиотека и демо-проект находятся в папке [Compilation](https://github.com/Zaharatot/DCTHashZ/tree/main/Compilation).
