using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TribalGroundedState : CharacterState
{
    private Animator animator;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        animator = character.Animator;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        blackboard.isGrounded = true;

        animator.SetTrigger("Ground");

        EditorDebug.Log("GROUNDED ENTER " + character.name);
    }

    protected override void OnExit() {
        base.OnExit();
        blackboard.isGrounded = false;

        animator.ResetTrigger("Ground");
        //EditorDebug.Log("GROUNDED EXIT");
    }

    protected override void OnUpdate()
    {
        if(!character.IsOnFloor(Reg.walkableLayerMask))
        {
            stateMachine.Trigger(Character.Trigger.Fall);
            return;
        }

        for(int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderJump)
            {
                stateMachine.Trigger(Character.Trigger.Jump);
                break;
            }
        }
    }
}
