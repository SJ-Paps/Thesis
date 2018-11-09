using System.Collections;
using System.Collections.Generic;
using SAM.FSM;
using UnityEngine;

public class TurretActionIdleState : CharacterState
{

    public TurretActionIdleState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard) : base(fsm, state, character, orders, blackboard)
    {

    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if(order == Character.Order.OrderAttack)
            {
                stateMachine.Trigger(Character.Trigger.Attack);
            }
        }
    }
}
