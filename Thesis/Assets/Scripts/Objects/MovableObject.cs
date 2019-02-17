using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovableObject : ActivableObject<Tribal> {

    protected Joint2D joint;
    protected new Rigidbody2D rigidbody2D;

    protected override void Awake()
    {
        base.Awake();

        joint = GetComponent<Joint2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public Joint2D GetJoint()
    {
        return joint;
    }

    public override void Activate(Tribal user)
    {
        if(joint.enabled == false)
        {
            joint.enabled = true;
            joint.connectedBody = user.RigidBody2D;
        }
        else
        {
            joint.enabled = false;
            joint.connectedBody = null;
        }
    }

    public override bool ShouldBeSaved()
    {
        return true;
    }

}
