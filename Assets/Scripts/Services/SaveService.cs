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

    //C��������� ������ � json ����
    public void SaveData(List<InventoryObjController> inventoryObjs)
    {
        JSONArray newSavedData = new JSONArray();
        foreach(var inventoryObj in inventoryObjs) newSavedData.Add(inventoryObj.Id);
        Debug.Log(newSavedData);
        File.WriteAllText(_filePath, newSavedData.ToString());
    }

    //��������� ���� � ������������ �������
    public JSONArray GetSavedData() => _jsonNode;

    //������ ����������� ������ �� �����
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
