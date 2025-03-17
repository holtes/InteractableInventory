using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.SerializableCollections;
using System.Linq;

public class BackpackController : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<ObjectTypes, Transform> _objectsPlaces;
    [SerializeField] private Transform _pointToDeattach;
    [SerializeField] private Transform _menuPivot;
    [SerializeField] private InventoryMenu _menu;
    [SerializeField] private float _magnetObjDuration = 1f;

    private Rigidbody _rigidbody;
    private Dictionary<ObjectTypes, InventoryObjController> _attachedObjects = new Dictionary<ObjectTypes, InventoryObjController>();
    private List<Collider> _overlapedObjects = new List<Collider>();

    public List<InventoryObjController> AttachedObjects { get => _attachedObjects.Values.ToList(); }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        MouseController.Inst.OnDragObjEnd += MoveObjectsToBagPoint;
    }

    //Сохранение всех объектов пересекающихся с рюкзаком
    private void OnTriggerEnter(Collider other) => _overlapedObjects.Add(other);

    //Удаление всех объектов вышедших из колайдера рюкзака
    private void OnTriggerExit(Collider other) => _overlapedObjects.Remove(other);

    //Присоединение объекта к рюкзаку
    private void AttachObjToBag(InventoryObjController inventoryObj)
    {
        _attachedObjects[inventoryObj.Type] = inventoryObj;
        inventoryObj.AttachObject(transform, _rigidbody);
        inventoryObj.IsPinnedToBag = true;
        inventoryObj.SetObjectPhysicState(true);
        _menu.SetInventoryImg(inventoryObj.Type, inventoryObj.Img);
        GameManager.OnChangeObjState?.Invoke(inventoryObj.Id, WebService.ActionType.Attached);
    }

    //Перемещение всех объектов в колайдере рюкзака к точке
    private void MoveObjectsToBagPoint()
    {
        foreach (var overlapObj in _overlapedObjects)
        {
            var inventoryObj = overlapObj.GetComponent<InventoryObjController>();
            if (!_attachedObjects.ContainsKey(inventoryObj.Type))
            {
                var objTarget = _objectsPlaces[inventoryObj.Type];
                inventoryObj.SetObjectPhysicState(false);
                inventoryObj.MoveToTarget(objTarget.position, objTarget.rotation, _magnetObjDuration, delegate { AttachObjToBag(inventoryObj); });
            }
        }      
    }

    //Отсоединение объекта от рюкзака
    public void DeattachObjFromBag(ObjectTypes objType)
    {
        var attachedObj = _attachedObjects[objType];
        attachedObj.DeattachObject();
        attachedObj.SetObjectPhysicState(false);
        attachedObj.MoveToTarget(_pointToDeattach.position, attachedObj.transform.rotation,
            _magnetObjDuration, delegate { attachedObj.SetObjectPhysicState(true); });
        attachedObj.IsPinnedToBag = false;
        _attachedObjects.Remove(objType);
        GameManager.OnChangeObjState?.Invoke(attachedObj.Id, WebService.ActionType.Deattached);
    }

    //Мгновенная установка и прикрепление объекта к рюкзаку
    public void SetObjToBagPlace(InventoryObjController inventoryObj)
    {
        inventoryObj.transform.position = _objectsPlaces[inventoryObj.Type].position;
        inventoryObj.transform.rotation = _objectsPlaces[inventoryObj.Type].rotation;
        inventoryObj.transform.localScale = _objectsPlaces[inventoryObj.Type].localScale;
        AttachObjToBag(inventoryObj);
    }

    //Открытие меню при зажатии ЛКМ на рюкзаке
    private void OnMouseDown() => _menu.OpenMenu();

    //Закрытие меню при зажатии ЛКМ на рюкзаке и выбор элемента
    private void OnMouseUp() => _menu.SelectItem();

    //Вращение рюкзака при наведении на него курсора
    private void OnMouseOver() => MouseController.Inst.RotateElement(transform);
    
    //Прикрепление меню к рюкзаку
    private void Update()
    {
        _menu.transform.position = _menuPivot.position;
        _menu.transform.rotation = _menuPivot.rotation;
    }
}
