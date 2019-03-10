﻿using UnityEngine;

public class RelativeJoint2DTuple : SJMonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    public RelativeJoint2D RelativeMe { get; private set; }
    public RelativeJoint2D RelativeOther { get; private set; }

    
    protected override void Awake()
    {
        base.Awake();

        RelativeMe = gameObject.AddComponent<RelativeJoint2D>();
        RelativeOther = gameObject.AddComponent<RelativeJoint2D>();

        RelativeMe.autoConfigureOffset = false;
        RelativeOther.autoConfigureOffset = false;

        RelativeMe.enabled = false;
        RelativeOther.enabled = false;

        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0;
    }

    public void Connect(Rigidbody2D me, Rigidbody2D other)
    {
        RelativeMe.enabled = true;
        RelativeOther.enabled = true;

        RelativeMe.connectedBody = me;
        RelativeOther.connectedBody = other;
    }

    public void Disconnect()
    {
        RelativeMe.connectedBody = null;
        RelativeOther.connectedBody = null;

        RelativeMe.enabled = false;
        RelativeOther.enabled = false;
    }
}
