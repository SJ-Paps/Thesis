using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TribalMovingState : TribalHSMState
{
    private const int rightDirection = 1;
    private const int leftDirection = -1;
    private const int noneDirection = 0;

    private Action onFixedUpdateDelegate;

    private int currentMoveDirection;
    private int previousMoveDirection;

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

        if(character.RigidBody2D.velocity.x == 0)
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

                currentMoveDirection = leftDirection;
                
                shouldMove = true;

                return TriggerResponse.Reject;

            case Character.Trigger.MoveRight:

                currentMoveDirection = rightDirection;
                
                shouldMove = true;

                return TriggerResponse.Reject;

            default:

                return TriggerResponse.Accept;

        }
    }

    private void OnFixedUpdate()
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

        currentMoveDirection = noneDirection;

        shouldMove = false; //reseteo la variable a false, si llega un evento esta se seteara a true y podra moverse
    }

    private void OnFirstUpdate()
    {
        float currentVelocity = character.RigidBody2D.velocity.x;

        if (LastEnteringTrigger == Character.Trigger.MoveRight || currentVelocity > 0)
        {
            MoveOnDirection(rightDirection);
        }
        else if (LastEnteringTrigger == Character.Trigger.MoveLeft || currentVelocity < 0)
        {
            MoveOnDirection(rightDirection * -1);
        }

        character.onFixedUpdate += onFixedUpdateDelegate;
    }

    private void MoveOnDirection(int direction)
    {
        ApplyForceOnDirection(direction);

        character.Face(direction < 0);
    }

    private void ApplyForceOnDirection(int direction)
    {
        character.RigidBody2D.AddForce(new Vector2(direction * character.Acceleration, 0), ForceMode2D.Impulse);
    }

    private void ClampVelocity(int lastDirection)
    {
        ApplyForceOnDirection(lastDirection * -1);
    }
}