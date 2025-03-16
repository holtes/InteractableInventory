using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public static Action<int, WebService.ActionType> OnChangeObjState;

    [SerializeField] private WebService _webService;
    [SerializeField] private SaveService _saveService;
    [SerializeField] private BackpackController _backpack;
    
    private List<InventoryObjController> _inventoryObjects;

    //Запуск сцены
    private void Start()
    {
        _inventoryObjects = FindObjectsOfType<InventoryObjController>().ToList();
        JSONArray savedData = _saveService.ReadDataFromSaveFile();
        LoadGame(savedData);

        OnChangeObjState += _webService.SendPostRequest;
    }

    //Загрузка предыдущей сессии
    private void LoadGame(JSONArray savedData)
    {
        foreach (var inventoryObjData in savedData)
        {
            var savedObj = _inventoryObjects.Find(obj => obj.Id == inventoryObjData.Value.AsInt);
            _backpack.SetObjToBagPlace(savedObj);
        }
    }

    //Сохранение сессии при выходе из игры
    private void OnDestroy() => _saveService.SaveData(_backpack.AttachedObjects);
}
