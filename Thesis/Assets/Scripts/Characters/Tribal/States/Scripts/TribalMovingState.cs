using System.Collections;
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

    public TribalMovingState()
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
        else if(Configuration.RigidBody2D.velocity.x > (velocityDeadZone * -1) && Configuration.RigidBody2D.velocity.x < velocityDeadZone)
        {
            SendEvent(Character.Order.StopMoving);
        }
        
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.onFixedUpdate -= onFixedUpdateDelegate;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        switch(trigger)
        {
            case Character.Order.MoveLeft:

                currentMoveDirection = (int)Vector2.left.x;
                
                shouldMove = true;

                return true;

            case Character.Order.MoveRight:

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

        if (Configuration.RigidBody2D.velocity.x > Owner.MaxVelocity.CurrentValue || Configuration.RigidBody2D.velocity.x < Owner.MaxVelocity.CurrentValue * -1)
        {
            ClampVelocity(currentMoveDirection);
        }

        previousMoveDirection = currentMoveDirection;

        currentMoveDirection = 0;

        shouldMove = false; //reseteo la variable a false, si llega un evento esta se seteara a true y podra moverse
    }

    private void OnFirstUpdate()
    {
        float currentVelocity = Configuration.RigidBody2D.velocity.x;

        if (LastEnteringTrigger == Character.Order.MoveRight || currentVelocity > 0)
        {
            MoveOnDirection((int)Vector2.right.x);
        }
        else if (LastEnteringTrigger == Character.Order.MoveLeft || currentVelocity < 0)
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
        Configuration.RigidBody2D.AddForce(new Vector2(direction * Configuration.Acceleration, 0), ForceMode2D.Impulse);
    }

    protected void ClampVelocity(int lastDirection)
    {
        ApplyForceOnDirection(lastDirection * -1);
    }
}