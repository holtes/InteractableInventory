using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Runtime.InteropServices;

public class SaveService : MonoBehaviour
{
    [SerializeField] private string _saveFileName;

    private string _filePath => Path.Combine(Application.persistentDataPath, _saveFileName);
    private JSONArray _jsonNode;

    // Импорт JavaScript-функций
    [DllImport("__Internal")]
    private static extern void SaveDataToLocalStorage(string key, string data);

    [DllImport("__Internal")]
    private static extern string LoadDataFromLocalStorage(string key);

    //Cохранение данных в json файл
    public void SaveData(List<InventoryObjController> inventoryObjs)
    {
        JSONArray newSavedData = new JSONArray();
        foreach(var inventoryObj in inventoryObjs) newSavedData.Add(inventoryObj.Id);
#if UNITY_EDITOR
        Debug.Log(newSavedData);
        File.WriteAllText(_filePath, newSavedData.ToString());
#else
        SaveDataToLocalStorage(_saveFileName, newSavedData.ToString());
#endif
    }

    //Получение нода с созраненными данными
    public JSONArray GetSavedData() => _jsonNode;

    //Чтение сохраненных данных из файла
    public JSONArray ReadDataFromSaveFile()
    {
#if UNITY_EDITOR
        if (!File.Exists(_filePath))
        {
            _jsonNode = new JSONArray();
            return _jsonNode;
        }
        string jsonStr = File.ReadAllText(_filePath);
        _jsonNode = JSONNode.Parse(jsonStr).AsArray;
        return _jsonNode;
#else
        string jsonStr = LoadDataFromLocalStorage(_saveFileName);
        if (string.IsNullOrEmpty(jsonStr))
        {
            _jsonNode = new JSONArray();
        }
        else
        {
            _jsonNode = JSONNode.Parse(jsonStr).AsArray;
        }
        return _jsonNode;
#endif
    }
}
