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

    //���������� ���� �������� �������������� � ��������
    private void OnTriggerEnter(Collider other) => _overlapedObjects.Add(other);

    //�������� ���� �������� �������� �� ��������� �������
    private void OnTriggerExit(Collider other) => _overlapedObjects.Remove(other);

    //������������� ������� � �������
    private void AttachObjToBag(InventoryObjController inventoryObj)
    {
        _attachedObjects[inventoryObj.Type] = inventoryObj;
        inventoryObj.AttachObject(transform, _rigidbody);
        inventoryObj.IsPinnedToBag = true;
        inventoryObj.SetObjectPhysicState(true);
        _menu.SetInventoryImg(inventoryObj.Type, inventoryObj.Img);
        GameManager.OnChangeObjState?.Invoke(inventoryObj.Id, WebService.ActionType.Attached);
    }

    //����������� ���� �������� � ��������� ������� � �����
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

    //������������ ������� �� �������
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

    //���������� ��������� � ������������ ������� � �������
    public void SetObjToBagPlace(InventoryObjController inventoryObj)
    {
        inventoryObj.transform.position = _objectsPlaces[inventoryObj.Type].position;
        inventoryObj.transform.rotation = _objectsPlaces[inventoryObj.Type].rotation;
        inventoryObj.transform.localScale = _objectsPlaces[inventoryObj.Type].localScale;
        AttachObjToBag(inventoryObj);
    }

    //�������� ���� ��� ������� ��� �� �������
    private void OnMouseDown() => _menu.OpenMenu();

    //�������� ���� ��� ������� ��� �� ������� � ����� ��������
    private void OnMouseUp() => _menu.SelectItem();

    //�������� ������� ��� ��������� �� ���� �������
    private void OnMouseOver() => MouseController.Inst.RotateElement(transform);
    
    //������������ ���� � �������
    private void Update()
    {
        _menu.transform.position = _menuPivot.position;
        _menu.transform.rotation = _menuPivot.rotation;
    }
}
