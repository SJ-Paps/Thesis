using System.Collections;
using System.Collections.Generic;
using SAM.FSM;
using UnityEngine;

public class TurretMovingState : CharacterState
{

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
                character.transform.Rotate(Vector3.forward, character.MovementVelocity * Time.deltaTime);
                //character.RigidBody2D.AddTorque(character.MovementVelocity, ForceMode2D.Force);
            }
            else if(order == Character.Order.OrderMoveRight)
            {
                character.transform.Rotate(Vector3.forward, -character.MovementVelocity * Time.deltaTime);
                //character.RigidBody2D.AddTorque(-character.MovementVelocity, ForceMode2D.Force);
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
