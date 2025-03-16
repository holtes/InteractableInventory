using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatController : InventoryObjController
{
    private FixedJoint _fixedJoint;

    //Метод присоединения шапки к рюкзаку
    public override void AttachObject(Transform root, Rigidbody parentRb)
    {
        base.AttachObject(root, parentRb);
        _fixedJoint = gameObject.AddComponent<FixedJoint>();
        _fixedJoint.connectedBody = parentRb;
    }

    //Метод отсоединения шапки от рюкзака
    public override void DeattachObject()
    {
        base.DeattachObject();
        Destroy(_fixedJoint);
        _fixedJoint = null;
    }
}
