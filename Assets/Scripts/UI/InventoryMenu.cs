using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools.SerializableCollections;
using UnityEngine.Events;
using DG.Tweening;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<ObjectTypes, InventoryItem> _inventoryItems;
    [SerializeField] private float _fadeDuration;

    public UnityEvent<ObjectTypes> OnItemSelected = new UnityEvent<ObjectTypes>();

    private SpriteRenderer _rootImg;
    private ObjectTypes _selectedType;

    private void Awake() => _rootImg = GetComponent<SpriteRenderer>();

    //Подписка на изменения наведения на элемент
    private void OnEnable()
    {
        foreach (var inventoryItem in _inventoryItems.values) inventoryItem.OnItemChangedState += SetSelectedItem;
    }

    //Отписка от изменения наведения на элемент
    private void OnDisable()
    {
        foreach (var inventoryItem in _inventoryItems.values) inventoryItem.OnItemChangedState -= SetSelectedItem;
    }

    //Метод выбора типа объекта в меню 
    private void SetSelectedItem(ObjectTypes type) => _selectedType = type;

    //Установка иконки меню в инвентаре
    public void SetInventoryImg(ObjectTypes type, Sprite objSprite)
    {
        _inventoryItems[type].SetSprite(objSprite);
    }

    //Выбор элемента меню
    public void SelectItem()
    {
        if (_selectedType == ObjectTypes.None || _inventoryItems[_selectedType].IsEmpty)
        {
            CloseMenu();
            return;
        }
        _inventoryItems[_selectedType].FadeElement(false, _fadeDuration);
        _inventoryItems[_selectedType].IsEmpty = true;
        OnItemSelected?.Invoke(_selectedType);
        CloseMenu();
    }

    //Открытие меню
    public void OpenMenu() => SetMenuState(true);

    //Закрытие меню
    public void CloseMenu() => SetMenuState(false);

    //Анимация закрытия/открытия меню
    private void SetMenuState(bool isOpen)
    {
        _rootImg.DOFade(isOpen ? 1 : 0, _fadeDuration);
        foreach (var inventoryItem in _inventoryItems.values)
        {
            if (!inventoryItem.IsEmpty) inventoryItem.FadeElement(isOpen, _fadeDuration);
        }
    }
}
