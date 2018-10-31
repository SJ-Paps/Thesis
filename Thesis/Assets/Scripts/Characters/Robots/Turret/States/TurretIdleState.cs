using System.Collections;
using System.Collections.Generic;
using SAM.FSM;
using UnityEngine;

public class TurretIdleState : CharacterState
{
    public TurretIdleState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard) : base(fsm, state, character, orders, blackboard)
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

            if (order == Character.Order.OrderMoveLeft || order == Character.Order.OrderMoveRight)
            {
                stateMachine.Trigger(Character.Trigger.Move);
            }
        }
    }

    protected override void OnExit()
    {

    }
}
