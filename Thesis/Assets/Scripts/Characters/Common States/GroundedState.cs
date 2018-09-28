using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundedState : CharacterState
{
    private Action<Collider2D> checkIsOnFloorDelegate;

    private float contactNormalOffsetY = 0.5f;
    private float contactNormalOffsetX = 0.2f;

    private BoxTrigger2D characterFeet;

    public GroundedState(FSM<Character.State, Character.Trigger> fsm,
       Character.State state,
       Character character,
       List<Character.Order> orderList,
       Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        checkIsOnFloorDelegate = CheckIsOnFloor;

        characterFeet = character.Feet;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        blackboard.isGrounded = true;

        characterFeet.onExited += checkIsOnFloorDelegate;

        animator.SetTrigger("Ground");

        EditorDebug.Log("GROUNDED ENTER " + character.name);
    }

    protected override void OnExit() {
        base.OnExit();
        blackboard.isGrounded = false;

        characterFeet.onExited -= checkIsOnFloorDelegate;

        animator.ResetTrigger("Ground");
        //EditorDebug.Log("GROUNDED EXIT");
    }

    protected override void OnUpdate()
    {
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

    private void CheckIsOnFloor(Collider2D collider)
    {
        if(collider.gameObject.layer == Reg.floorLayer || collider.gameObject.layer == Reg.objectLayer)
        {
            stateMachine.Trigger(Character.Trigger.Fall);
        }
    }
}
