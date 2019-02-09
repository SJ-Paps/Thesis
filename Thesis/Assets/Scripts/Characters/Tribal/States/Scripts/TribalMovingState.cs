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

    public TribalMovingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        onFixedUpdateDelegate = OnFixedUpdate;

        activeDebug = true;
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
        else if(character.RigidBody2D.velocity.x > (velocityDeadZone * -1) && character.RigidBody2D.velocity.x < velocityDeadZone)
        {
            SendEvent(Character.Trigger.StopMoving);
        }
        
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.onFixedUpdate -= onFixedUpdateDelegate;
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        switch(trigger)
        {
            case Character.Trigger.MoveLeft:

                currentMoveDirection = (int)Vector2.left.x;
                
                shouldMove = true;

                return TriggerResponse.Reject;

            case Character.Trigger.MoveRight:

                currentMoveDirection = (int)Vector2.right.x;
                
                shouldMove = true;

                return TriggerResponse.Reject;

            default:

                return TriggerResponse.Accept;

        }
    }

    protected virtual void OnFixedUpdate()
    {
        if(shouldMove)
        {
            MoveOnDirection(currentMoveDirection);
        }

        if (character.RigidBody2D.velocity.x > character.MaxMovementVelocity || character.RigidBody2D.velocity.x < character.MaxMovementVelocity * -1)
        {
            ClampVelocity(currentMoveDirection);
        }

        previousMoveDirection = currentMoveDirection;

        currentMoveDirection = 0;

        shouldMove = false; //reseteo la variable a false, si llega un evento esta se seteara a true y podra moverse
    }

    private void OnFirstUpdate()
    {
        float currentVelocity = character.RigidBody2D.velocity.x;

        if (LastEnteringTrigger == Character.Trigger.MoveRight || currentVelocity > 0)
        {
            MoveOnDirection((int)Vector2.right.x);
        }
        else if (LastEnteringTrigger == Character.Trigger.MoveLeft || currentVelocity < 0)
        {
            MoveOnDirection((int)Vector2.left.x);
        }

        character.onFixedUpdate += onFixedUpdateDelegate;
    }

    protected void MoveOnDirection(int direction)
    {
        ApplyForceOnDirection(direction);

        OnMoving();
    }

    protected virtual void OnMoving()
    {
        character.Face(currentMoveDirection < 0);
    }

    protected void ApplyForceOnDirection(int direction)
    {
        character.RigidBody2D.AddForce(new Vector2(direction * character.Acceleration, 0), ForceMode2D.Impulse);
    }

    protected void ClampVelocity(int lastDirection)
    {
        ApplyForceOnDirection(lastDirection * -1);
    }
}