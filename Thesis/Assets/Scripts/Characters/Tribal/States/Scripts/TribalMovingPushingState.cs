using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalMovingPushingState : TribalMovingState
{
    private int facingDirection;

    public TribalMovingPushingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        facingDirection = (int)character.transform.right.x;
    }

    protected override void OnUpdate()
    {
        if (character.RigidBody2D.velocity.x < 0 && facingDirection > 0
            || character.RigidBody2D.velocity.x > 0 && facingDirection < 0)
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