mergeInto(LibraryManager.library, {
    SaveDataToLocalStorage: function (key, data) {
        var keyStr = UTF8ToString(key);
        var dataStr = UTF8ToString(data);
        localStorage.setItem(keyStr, dataStr);
    },

    LoadDataFromLocalStorage: function (key) {
        var keyStr = UTF8ToString(key);
        var data = localStorage.getItem(keyStr);
        if (data === null) return 0; // Возвращаем 0, если данных нет
        var buffer = _malloc(data.length + 1);
        stringToUTF8(data, buffer, data.length + 1);
        return buffer;
    },

    RemoveDataFromLocalStorage: function (key) {
        var keyStr = UTF8ToString(key);
        localStorage.removeItem(keyStr);
    },

    RegisterBeforeUnload: function () {
        window.onbeforeunload = function () {
            // Вызов Unity-функции для сохранения данных
            unityInstance.SendMessage('GameManager', 'SaveDataBeforeUnload');
            return null; // Браузер покажет стандартное предупреждение
        };
    }
});