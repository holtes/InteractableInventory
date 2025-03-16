using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class SaveService : MonoBehaviour
{
    [SerializeField] private string _saveFileName;

    private string _filePath => Path.Combine(Application.persistentDataPath, _saveFileName);
    private JSONArray _jsonNode;

    //Cохранение данных в json файл
    public void SaveData(List<InventoryObjController> inventoryObjs)
    {
        JSONArray newSavedData = new JSONArray();
        foreach(var inventoryObj in inventoryObjs) newSavedData.Add(inventoryObj.Id);
        Debug.Log(newSavedData);
        File.WriteAllText(_filePath, newSavedData.ToString());
    }

    //Получение нода с созраненными данными
    public JSONArray GetSavedData() => _jsonNode;

    //Чтение сохраненных данных из файла
    public JSONArray ReadDataFromSaveFile()
    {
        if (!File.Exists(_filePath))
        {
            _jsonNode = new JSONArray();
            return _jsonNode;
        }
        string jsonStr = File.ReadAllText(_filePath);
        _jsonNode = JSONNode.Parse(jsonStr).AsArray;
        return _jsonNode;
    } 
}
