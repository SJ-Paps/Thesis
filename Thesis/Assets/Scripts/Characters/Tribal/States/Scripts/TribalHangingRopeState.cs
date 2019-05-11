﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalHangingRopeState : TribalHSMState
{
    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Blackboard.ropeHandler = new GameObject("RopeHandler").AddComponent<RelativeJoint2DTuple>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Rope rope = (Rope)Blackboard.activable;

        Owner.RigidBody2D.velocity = new Vector2(0, 0);

        Rigidbody2D nearestSegment = rope.GetNearestSegment(Owner.transform.position);

        nearestSegment.velocity = new Vector2(0, 0);

        Blackboard.ropeHandler.Rigidbody2D.MovePosition(nearestSegment.position);

        Blackboard.ropeHandler.Connect(Owner.RigidBody2D, nearestSegment);

        Blackboard.ropeHandler.RelativeMe.autoConfigureOffset = false;
        Blackboard.ropeHandler.RelativeOther.autoConfigureOffset = false;

        Blackboard.ropeHandler.RelativeMe.maxForce = 300;
        Blackboard.ropeHandler.RelativeOther.maxForce = 300;

        Blackboard.ropeHandler.RelativeMe.linearOffset = new Vector2(0, 0);
        Blackboard.ropeHandler.RelativeOther.linearOffset = new Vector2(0, 0);
    }

    protected override void OnExit()
    {
        base.OnExit();

        Blackboard.ropeHandler.Disconnect();
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Jump)
        {
            SendEvent(Character.Order.Fall);
            return true;
        }

        return false;
    }
}