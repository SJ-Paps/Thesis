using System.Collections;
using System.Collections.Generic;
using SAM.FSM;
using UnityEngine;
using System;

[Serializable]
public class TurretMovingState : CharacterState
{
    [SerializeField]
    private float leftLimit, rightLimit;

    private float currentRotationReference;

    public TurretMovingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard) : base(fsm, state, character, orders, blackboard)
    {

    }

    protected override void OnEnter()
    {

    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft)
            {
                if(currentRotationReference < leftLimit)
                {
                    float rotation = character.MovementVelocity * Time.deltaTime;

                    if(currentRotationReference + rotation > leftLimit)
                    {
                        rotation = leftLimit - currentRotationReference;
                    }

                    character.transform.Rotate(Vector3.forward, rotation);
                    currentRotationReference += rotation;
                }
            }
            else if(order == Character.Order.OrderMoveRight)
            {
                if(currentRotationReference > rightLimit)
                {
                    float rotation = -character.MovementVelocity * Time.deltaTime;

                    if (currentRotationReference + rotation < rightLimit)
                    {
                        rotation = rightLimit - currentRotationReference;
                    }

                    character.transform.Rotate(Vector3.forward, rotation);
                    currentRotationReference += rotation;
                }
                
            }
            else
            {
                stateMachine.Trigger(Character.Trigger.StopMoving);
            }
        }
    }

    protected override void OnExit()
    {
        
    }
}
