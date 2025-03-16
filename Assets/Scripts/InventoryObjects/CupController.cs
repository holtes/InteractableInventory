using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : InventoryObjController
{
    [SerializeField] private Vector3 _cupAnchor;

    private SpringJoint _springJoint;

    //Метод присоединения кружки к рюкзаку
    public override void AttachObject(Transform root, Rigidbody parentRb)
    {
        base.AttachObject(root, parentRb);
        _springJoint = gameObject.AddComponent<SpringJoint>();
        _springJoint.connectedBody = parentRb;
        _springJoint.anchor = _cupAnchor;
        _springJoint.spring = 100f;
        _springJoint.damper = 1f;
    }

    //Метод отсоединения кружки от рюкзака
    public override void DeattachObject()
    {
        base.DeattachObject();
        Destroy(_springJoint);
        _springJoint = null;
    }
}
