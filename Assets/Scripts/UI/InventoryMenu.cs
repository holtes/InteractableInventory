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

    //�������� �� ��������� ��������� �� �������
    private void OnEnable()
    {
        foreach (var inventoryItem in _inventoryItems.values) inventoryItem.OnItemChangedState += SetSelectedItem;
    }

    //������� �� ��������� ��������� �� �������
    private void OnDisable()
    {
        foreach (var inventoryItem in _inventoryItems.values) inventoryItem.OnItemChangedState -= SetSelectedItem;
    }

    //����� ������ ���� ������� � ���� 
    private void SetSelectedItem(ObjectTypes type) => _selectedType = type;

    //��������� ������ ���� � ���������
    public void SetInventoryImg(ObjectTypes type, Sprite objSprite)
    {
        _inventoryItems[type].SetSprite(objSprite);
    }

    //����� �������� ����
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

    //�������� ����
    public void OpenMenu() => SetMenuState(true);

    //�������� ����
    public void CloseMenu() => SetMenuState(false);

    //�������� ��������/�������� ����
    private void SetMenuState(bool isOpen)
    {
        _rootImg.DOFade(isOpen ? 1 : 0, _fadeDuration);
        foreach (var inventoryItem in _inventoryItems.values)
        {
            if (!inventoryItem.IsEmpty) inventoryItem.FadeElement(isOpen, _fadeDuration);
        }
    }
}
