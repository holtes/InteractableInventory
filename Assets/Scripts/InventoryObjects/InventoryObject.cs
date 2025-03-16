using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryObject", menuName = "Inventory Data", order = 51)]
public class InventoryObject : ScriptableObject
{
    [Header("Вес")]
    public float Weight;
    [Header("Имя")]
    public string Name;
    [Header("Тип")]
    public ObjectTypes Type;
    [Header("Иконка в меню")]
    public Sprite Img;
}
