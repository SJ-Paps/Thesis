using System.Collections;
using System.Collections.Generic;
using SAM.FSM;
using UnityEngine;

public class TurretMovingState : CharacterState
{
    private Rigidbody2D rigidbody2D;

    public TurretMovingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard) : base(fsm, state, character, orders, blackboard)
    {
        rigidbody2D = character.RigidBody2D;
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
                character.transform.Rotate(Vector3.forward, character.MovementVelocity);
            }
            else if(order == Character.Order.OrderMoveRight)
            {
                character.transform.Rotate(Vector3.forward, -character.MovementVelocity);
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
