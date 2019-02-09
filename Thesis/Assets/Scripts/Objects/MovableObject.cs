using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovableObject : SJMonoBehaviour {

    protected Joint2D joint;

    protected override void Awake()
    {
        base.Awake();

        joint = GetComponent<Joint2D>();
    }

    public void Connect(Rigidbody2D rigidbody)
    {
        joint.enabled = true;
        joint.connectedBody = rigidbody;
    }

    public void Disconnect()
    {
        joint.enabled = false;
        joint.connectedBody = null;
    }

    public Joint2D GetJoint()
    {
        return joint;
    }

}
