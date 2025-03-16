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

    //�������/�������� �������� ����
    public void FadeElement(bool isOpen, float fadeDuration) => _itemImg.DOFade(isOpen? 1 : 0, fadeDuration);

    //��������� ������� �������� ����
    public void SetSprite(Sprite sprite)
    {
        _itemImg.sprite = sprite;
        IsEmpty = false;
    }

    //�������� ���� ������� ��� ��������� �� ����
    private void OnMouseEnter() => OnItemChangedState?.Invoke(_slotType);

    //�������� ������� ���� ��� ����� ������� � �������
    private void OnMouseExit() => OnItemChangedState?.Invoke(ObjectTypes.None);
}
