using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryObject", menuName = "Inventory Data", order = 51)]
public class InventoryObject : ScriptableObject
{
    [Header("���")]
    public float Weight;
    [Header("���")]
    public string Name;
    [Header("���")]
    public ObjectTypes Type;
    [Header("������ � ����")]
    public Sprite Img;
}
