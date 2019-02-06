using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TribalMovingState : TribalHSMState
{
    private const int rightDirection = 1;
    private const int noneDirection = 0;

    private Action onFixedUpdateDelegate;

    private int currentMoveDirection;
    private int previousMoveDirection;

    private bool shouldMove;

    public TribalMovingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        float currentVelocity = character.RigidBody2D.velocity.x;
        
        if(LastTrigger == Character.Trigger.MoveRight || currentVelocity > 0)
        {
            Kick(rightDirection);
        }
        else if(LastTrigger == Character.Trigger.MoveLeft || currentVelocity < 0)
        {
            Kick(rightDirection * -1);
        }

        character.onFixedUpdate += onFixedUpdateDelegate;

        EditorDebug.Log("MOVING ENTER " + character.name);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(character.RigidBody2D.velocity.x == 0)
        {
            SendEvent(Character.Trigger.StopMoving);
        }

        
        
        
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.onFixedUpdate -= onFixedUpdateDelegate;

        EditorDebug.Log("MOVING EXIT " + character.name);
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        switch(trigger)
        {
            case Character.Trigger.MoveLeft:

                currentMoveDirection = rightDirection * -1;
                
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
            if (previousMoveDirection != currentMoveDirection)
            {
                //posible animacion de cambiar de direccion de movimiento
                MoveOnDirection(currentMoveDirection);
            }
            else
            {
                MoveOnDirection(currentMoveDirection);
            }

            if (character.RigidBody2D.velocity.x > character.MaxMovementVelocity || character.RigidBody2D.velocity.x < character.MaxMovementVelocity * -1)
            {
                ClampVelocity(currentMoveDirection);
            }
        }

        previousMoveDirection = currentMoveDirection;

        currentMoveDirection = noneDirection;
    }

    private void MoveOnDirection(int direction)
    {
        character.RigidBody2D.AddForce(new Vector2(direction * character.MovementVelocity, 0), ForceMode2D.Impulse);
    }

    private void ClampVelocity(int lastDirection)
    {
        MoveOnDirection(lastDirection * -1);
    }

    private void Kick(int direction)
    {
        character.RigidBody2D.AddForce(new Vector2(direction * character.MovementVelocity / 2, 0), ForceMode2D.Impulse);
    }
}