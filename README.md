# Интерактивный инвентарь 

Этот проект содержит настроенную сцену с инвентарём и объектами, которые могут взаимодейтсовать с ним.

## Используемые технологии

- **Unity**: Игровой движок.
- **C#**: Основной язык программирования.
- **DOTween**: Библиотека для создания плавных анимаций в Runtime.
- **Unitask**: Библиотека для работы ассинхронностью в Unity.
- **SimpleJson**: Библиотека для работы с JSON (загрузка/выгрузка данных из файлов).

## Возможности и управление
- Зажимая `ЛКМ` на любом из объектов и двигая мышкой, вы можете перетаскивать его по сцене.
- Наведя мышку на `рюкзак` и начав крутить колесико мыши, рыкзак будет вращаться вокруг своей оси.
- Перетаскивая предмет и крутя колесико мыши, вы можете двигать объект по оси Z.
- При зажатии `ЛКМ` на `рюкзаке` над ним появляется меню с предметами, прикрепленными к нему.
- Продолжая нажимать `ЛКМ` при активном меню и наводя мышь на иконку объекта, вы можете вытащить объект из инвентаря.
- Объекты имеют общий базовый класс `InventoryObjController` (реализует основные функции взаимодействия с инвентарем).
- Данные по объектам хранятся и настраиваются через `ScriptableObject` объекта `InventoryObject`.
- Созраненные данные сцены хранятся по пути `Application.persistentDataPath\имя файла настраиваемого на объекте GameManager`.

## Ссылка на проект
https://psyonik.itch.io/inventorytestwork
