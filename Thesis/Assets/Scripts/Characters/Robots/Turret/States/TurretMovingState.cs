using SAM.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TurretMovingState : CharacterState
{
    private Turret turret;

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
                float rotation = character.MovementVelocity * Time.deltaTime;

                turret.Rotate(rotation);
            }
            else if(order == Character.Order.OrderMoveRight)
            {
                float rotation = -character.MovementVelocity * Time.deltaTime;

                turret.Rotate(rotation);
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

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        turret = (Turret)character;
    }
}
