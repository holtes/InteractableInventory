using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _moveSpeed = 1f;

    private Vector3 _mousePosition;
    private float _zDimension;
    private InventoryObjController _attachedObj;

    public static MouseController Inst;
    public Action OnDragObjEnd;

    private void Awake() => Inst = this;

    //Получение позиции мышки на объекте
    public Vector3 GetObjMousePosition(Vector3 objPosition) => Camera.main.WorldToScreenPoint(objPosition);

    //Присоединение объекта к мышке
    public void AttachObjToMouse(InventoryObjController inventoryObject)
    {
        _mousePosition = Input.mousePosition - GetObjMousePosition(inventoryObject.transform.position);
        _zDimension = inventoryObject.transform.position.z;
        inventoryObject.SetObjectPhysicState(false);
        _attachedObj = inventoryObject;
    }

    //Отсоединение объекта от мышки
    public void DeattachObjFromMouse()
    {
        _attachedObj.SetObjectPhysicState(true);
        _attachedObj = null;
    }

    //Установка позиции прикрепленного объекта при перемещении мышки
    public void SetMousePointPos()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) _zDimension += scroll * _moveSpeed;
        Vector3 newObjPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePosition);
        _attachedObj.transform.position = new Vector3(newObjPosition.x, newObjPosition.y, _zDimension);
    }

    //Поворот объекта вокруг себя по колесику мыши
    public void RotateElement(Transform transform)
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) transform.Rotate(Vector3.up, scroll * _rotationSpeed);
    }
}
