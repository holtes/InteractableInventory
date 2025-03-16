using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private ObjectTypes _slotType;
    
    private SpriteRenderer _itemImg;

    public Action<ObjectTypes> OnItemChangedState;
    
    [HideInInspector] public bool IsEmpty = true;

    private void Awake() => _itemImg = GetComponent<SpriteRenderer>();

    //Скрытие/Открытие элемента меню
    public void FadeElement(bool isOpen, float fadeDuration) => _itemImg.DOFade(isOpen? 1 : 0, fadeDuration);

    //Установка спрайта элемента меню
    public void SetSprite(Sprite sprite)
    {
        _itemImg.sprite = sprite;
        IsEmpty = false;
    }

    //Отправка типа объекта при наведении на него
    private void OnMouseEnter() => OnItemChangedState?.Invoke(_slotType);

    //Отправка пустого типа при уходе курсора с объекта
    private void OnMouseExit() => OnItemChangedState?.Invoke(ObjectTypes.None);
}
