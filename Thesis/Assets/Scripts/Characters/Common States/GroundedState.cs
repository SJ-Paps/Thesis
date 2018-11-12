using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundedState : CharacterState
{
    private float contactNormalOffsetY = 0.5f;
    private float contactNormalOffsetX = 0.2f;

    private int floorLayers = (1 << Reg.floorLayer) | (1 << Reg.objectLayer);

    private Animator animator;

    public GroundedState(FSM<Character.State, Character.Trigger> fsm,
       Character.State state,
       Character character,
       List<Character.Order> orderList,
       Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
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
        if(!character.IsOnFloor(floorLayers))
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
