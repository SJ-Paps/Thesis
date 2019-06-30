using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalMovingPushingState : TribalMovingState
{
    private int facingDirection;
    

    protected override void OnEnter()
    {
        base.OnEnter();

        facingDirection = (int)Owner.transform.right.x;
    }

    protected override void OnUpdate()
    {
        if (Configuration.RigidBody2D.velocity.x < 0 && facingDirection > 0
            || Configuration.RigidBody2D.velocity.x > 0 && facingDirection < 0)
        {
            SendEvent(Character.Order.StopPushing);
        }
        else
        {
            SendEvent(Character.Order.Push);
        }

        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnMoving()
    {

    }
}