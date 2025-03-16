using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class InventoryObjController : MonoBehaviour
{
    [SerializeField] private InventoryObject _inventoryObject;
    [SerializeField] protected int _id;
    protected float _weight { get => _inventoryObject.Weight; }
    protected string _name { get => _inventoryObject.Name; }
    protected ObjectTypes _type { get => _inventoryObject.Type; }
    protected Sprite _img { get => _inventoryObject.Img; }

    [HideInInspector] public ObjectTypes Type => _type;
    [HideInInspector] public Sprite Img => _img;
    [HideInInspector] public bool IsPinnedToBag { set => _isPinnedToBag = value; }
    [HideInInspector] public int Id { get => _id; }


    private bool _isPinnedToBag = false;
    private Transform _attachedRoot;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = _weight;
    }

    //Метод присоединения объекта инвентаря к рюкзаку
    public virtual void AttachObject(Transform root, Rigidbody parentRb)
    {
        _attachedRoot = root;
        transform.SetParent(root);
    }

    //Метод отсоединения объекта инвентаря от рюкзака
    public virtual void DeattachObject ()
    {
        transform.SetParent(_attachedRoot.parent);
        _attachedRoot = null;
    }

    //Метод запускающий корутину перемещения объекта к точке
    public void MoveToTarget(Vector3 targetPosition, Quaternion targetRotation, float magnetDuration, Action OnMoveComplete = null) => 
        StartCoroutine(MoveToTargetIE(targetPosition, targetRotation, magnetDuration, OnMoveComplete));

    //Корутина перемещения объекта к точке
    private IEnumerator MoveToTargetIE(Vector3 targetPosition, Quaternion targetRotation,  float magnetDuration, Action OnMoveComplete)
    {
        var moveTween = transform.DOMove(targetPosition, magnetDuration);
        var rotateTween = transform.DORotateQuaternion(targetRotation, magnetDuration);

        yield return moveTween.WaitForCompletion();
        yield return rotateTween.WaitForCompletion();
        OnMoveComplete?.Invoke();
    }

    //Включение/Выключение физики объекта
    public void SetObjectPhysicState(bool state)
    {
        _rigidbody.isKinematic = !state;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }


    //Присоединение объекта к мышке при нажатии на него
    private void OnMouseDown()
    {
        if (_isPinnedToBag) return;
        MouseController.Inst.AttachObjToMouse(this);
    }

    //Drag объекта
    private void OnMouseDrag()
    {
        if (_isPinnedToBag) return;
        MouseController.Inst.SetMousePointPos();
    }

    //Отсоединение объекта от мышки
    private void OnMouseUp()
    {
        if (_isPinnedToBag) return;
        MouseController.Inst.DeattachObjFromMouse();
        MouseController.Inst.OnDragObjEnd?.Invoke();
    }
    

}
