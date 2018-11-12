using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TribalIdleState : CharacterState
{
    private Animator animator;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        animator = character.Animator;
    }

    protected override void OnEnter()
    {
        animator.SetTrigger("Idle");

        EditorDebug.Log("IDLE ENTER");
    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft || order == Character.Order.OrderMoveRight)
            {
                stateMachine.Trigger(Character.Trigger.Move);
                break;
            }
        }
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Idle");
    }
}
