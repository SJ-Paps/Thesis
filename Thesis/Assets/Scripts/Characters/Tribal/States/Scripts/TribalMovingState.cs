﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TribalMovingState : TribalHSMState
{
    private Action onFixedUpdateDelegate;

    private int currentMoveDirection;
    private int previousMoveDirection;

    private float velocityDeadZone = 0.0002f;

    private bool shouldMove;

    private bool isFirstUpdate;

    public TribalMovingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        isFirstUpdate = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(isFirstUpdate)
        {
            OnFirstUpdate();

            isFirstUpdate = false;
        }
        else if(Owner.RigidBody2D.velocity.x > (velocityDeadZone * -1) && Owner.RigidBody2D.velocity.x < velocityDeadZone)
        {
            SendEvent(Character.Trigger.StopMoving);
        }
        
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.onFixedUpdate -= onFixedUpdateDelegate;
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        switch(trigger)
        {
            case Character.Trigger.MoveLeft:

                currentMoveDirection = (int)Vector2.left.x;
                
                shouldMove = true;

                return true;

            case Character.Trigger.MoveRight:

                currentMoveDirection = (int)Vector2.right.x;
                
                shouldMove = true;

                return true;

            default:

                return false;

        }
    }

    protected virtual void OnFixedUpdate()
    {
        if(shouldMove)
        {
            MoveOnDirection(currentMoveDirection);
        }

        if (Owner.RigidBody2D.velocity.x > Owner.MaxVelocity.CurrentValue || Owner.RigidBody2D.velocity.x < Owner.MaxVelocity.CurrentValue * -1)
        {
            ClampVelocity(currentMoveDirection);
        }

        previousMoveDirection = currentMoveDirection;

        currentMoveDirection = 0;

        shouldMove = false; //reseteo la variable a false, si llega un evento esta se seteara a true y podra moverse
    }

    private void OnFirstUpdate()
    {
        float currentVelocity = Owner.RigidBody2D.velocity.x;

        if (LastEnteringTrigger == Character.Trigger.MoveRight || currentVelocity > 0)
        {
            MoveOnDirection((int)Vector2.right.x);
        }
        else if (LastEnteringTrigger == Character.Trigger.MoveLeft || currentVelocity < 0)
        {
            MoveOnDirection((int)Vector2.left.x);
        }

        Owner.onFixedUpdate += onFixedUpdateDelegate;
    }

    protected void MoveOnDirection(int direction)
    {
        ApplyForceOnDirection(direction);

        OnMoving();
    }

    protected virtual void OnMoving()
    {
        Owner.Face(currentMoveDirection < 0);
    }

    protected void ApplyForceOnDirection(int direction)
    {
        Owner.RigidBody2D.AddForce(new Vector2(direction * Owner.Acceleration, 0), ForceMode2D.Impulse);
    }

    protected void ClampVelocity(int lastDirection)
    {
        ApplyForceOnDirection(lastDirection * -1);
    }
}