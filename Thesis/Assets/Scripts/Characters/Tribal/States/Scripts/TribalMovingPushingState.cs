using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalMovingPushingState : TribalMovingState
{
    private int facingDirection;

    public TribalMovingPushingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        facingDirection = (int)Owner.transform.right.x;
    }

    protected override void OnUpdate()
    {
        if (Owner.RigidBody2D.velocity.x < 0 && facingDirection > 0
            || Owner.RigidBody2D.velocity.x > 0 && facingDirection < 0)
        {
            SendEvent(Character.Trigger.StopPushing);
        }
        else
        {
            SendEvent(Character.Trigger.Push);
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