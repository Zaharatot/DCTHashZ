# DCTHashZ

#### EN description:

This library implements the image comparison functionality and is intended for applications written in C#.
The reason for writing my own implementation is quite simple — I just could not find any analogues that would allow a developer to do whatever with the library (e.g., [pHash library](https://github.com/pgrho/phash) is distributed under the GPL, therefore, it cannot be used in any kind of commercial projects without publishing their source code in the open domain). However, I was in the middle of writing my own project which, in theory, can become a commercial one in the future, and did not want any complications involved with using pHash library. Having found no alternatives, I created my own implementation which is totally open and free — you can do whatever you want with the code provided (within the boundaries of Apache 2.0 license).

Comparison is implemented by the way of hash generation via Discrete Cosine Transform Based Hash, also known as pHash method. Materials used were mainly taken from [this](https://habr.com/ru/post/237307/) and several other posts on Habr.com. Also, Wikipedia and several scientific works on the topis were utilised (regretfully, I did not save the relevant links). In addition, the results obtained were compared to the output of [demo pHash](https://www.phash.org/demo/), wherein I tried to match my results to the baseline ones.

The principles of operating this library are quite simple. There is "DCTHash" facade class, which comprises all interaction methods for this library. To create a hash generation request it is necessary to invoke the "AddTasks" method, supplying it with a list of paths to folders containing image files. To obtain the results it is necessary to subscribe to the "CompleteCeneration" event, it will output a list of results comprising a file name, a path to the file, hash of the indicated image and hash generation status. To track the result of hash generation you can subscribe to the "UpdateCreationStatus" event, it will return consolidated information on the generation process. In particular, the output will comprise a number of tasks for generation process, a number of tasks being performed, a number of completed tasks and a total number of tasks in the list.

For a more on-hands example you can peruse a demo project ("DCTHashZDemo") by downloading this repository and opening the solution. There are 2 operating modes — a simple hash generation mode and a file comparison mode. The compiled library and the demo project can be found in the [Compilation](https://github.com/Zaharatot/DCTHashZ/tree/main/Compilation) folder.


#### RU Description/Описание на русском:

Данная библиотека реализует функционал сравнения изображений, и предназначена для приложений, написанных на языке C#.
Причина написания своей реализации проста — я просто не нашёл аналогов, которые давали бы разработчику возможность делать с библиотекой что угодно ([библиотека pHash](https://github.com/pgrho/phash) распространяется под GPL, и это означает что его нельзя использовать в коммерческих проектах, не публикуя в открытую их исходный код). Я же, писал свой небольшой проект, который в теории мог дорасти до коммерческого, и не хотел этих проблем с библиотекой pHash. Не найдя других альтернатив, я создал свою реализацию, которая будет полностью открытой — делайте с ней что хотите (в рамках лицензии Apache 2.0).

Сравнение реализовано через генерацию хеша, путём дискретного косинусного преобразования (Discrete Cosine Transform Based Hash), оно же — pHash. Материалы брались в основном из [этой](https://habr.com/ru/post/237307/) и ещё нескольких статей на Хабре. Плюс — Wiki, и несколько научных работ по теме (на которые я, к сожалению, не сохранил ссылок). Дополнительно, я сверял получаемые результаты с [demo pHash](https://www.phash.org/demo/), стараясь получить близкие к эталонным.

Принцип работы с данной библиотекой довольно прост. Есть фасадный класс "DCTHash", в который вынесены все методы общения с этой библиотекой. Для создания запроса на генерацию хешей, необходимо вызвать метод "AddTasks", передав в него список путей к файлам изображений. Для получения результатов необходимо подписаться на ивент "CompleteCeneration" — он вернёт список результатов, в котором будет имя файла, путь к нему, хеш этого изображения и статус генерации хеша. Для отслеживания результатов генерации хеша, можно подписаться на событие "UpdateCreationStatus" — оно будет возвращать комплексную информацию о процессе генерации. В частности — количество задач, ожидающих генерации, количество задач в работе, количество завершённых задач и общее количество задач в списке.

Более подробно посмотреть на это можно в демо-проекте ("DCTHashZDemo"), скачав этот репозиторий и открыв решение. Там есть 2 варианта работы — простая генерация хешей, и сравнение файлов. Собранные библиотека и демо-проект можно найти в папке [Compilation](https://github.com/Zaharatot/DCTHashZ/tree/main/Compilation).
