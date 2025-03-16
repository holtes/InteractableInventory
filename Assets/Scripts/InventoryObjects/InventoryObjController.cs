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

    //����� ������������� ������� ��������� � �������
    public virtual void AttachObject(Transform root, Rigidbody parentRb)
    {
        _attachedRoot = root;
        transform.SetParent(root);
    }

    //����� ������������ ������� ��������� �� �������
    public virtual void DeattachObject ()
    {
        transform.SetParent(_attachedRoot.parent);
        _attachedRoot = null;
    }

    //����� ����������� �������� ����������� ������� � �����
    public void MoveToTarget(Vector3 targetPosition, Quaternion targetRotation, float magnetDuration, Action OnMoveComplete = null) => 
        StartCoroutine(MoveToTargetIE(targetPosition, targetRotation, magnetDuration, OnMoveComplete));

    //�������� ����������� ������� � �����
    private IEnumerator MoveToTargetIE(Vector3 targetPosition, Quaternion targetRotation,  float magnetDuration, Action OnMoveComplete)
    {
        var moveTween = transform.DOMove(targetPosition, magnetDuration);
        var rotateTween = transform.DORotateQuaternion(targetRotation, magnetDuration);

        yield return moveTween.WaitForCompletion();
        yield return rotateTween.WaitForCompletion();
        OnMoveComplete?.Invoke();
    }

    //���������/���������� ������ �������
    public void SetObjectPhysicState(bool state)
    {
        _rigidbody.isKinematic = !state;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }


    //������������� ������� � ����� ��� ������� �� ����
    private void OnMouseDown()
    {
        if (_isPinnedToBag) return;
        MouseController.Inst.AttachObjToMouse(this);
    }

    //Drag �������
    private void OnMouseDrag()
    {
        if (_isPinnedToBag) return;
        MouseController.Inst.SetMousePointPos();
    }

    //������������ ������� �� �����
    private void OnMouseUp()
    {
        if (_isPinnedToBag) return;
        MouseController.Inst.DeattachObjFromMouse();
        MouseController.Inst.OnDragObjEnd?.Invoke();
    }
    

}
