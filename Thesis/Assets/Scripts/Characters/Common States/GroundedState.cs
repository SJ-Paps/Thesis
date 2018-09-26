﻿using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundedState : CharacterState
{
    private Action<Collision2D> checkIsOnFloorDelegate;

    public GroundedState(FSM<Character.State, Character.Trigger> fsm,
       Character.State state,
       Character character,
       List<Character.Order> orderList,
       Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        checkIsOnFloorDelegate = CheckIsOnFloor;

        
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        blackboard.isGrounded = true;

        character.onCollisionStay2D += CheckIsOnFloor;

        animator.SetTrigger("Ground");

        EditorDebug.Log("GROUNDED ENTER");
    }

    protected override void OnExit() {
        base.OnExit();
        blackboard.isGrounded = false;

        character.onCollisionStay2D -= CheckIsOnFloor;

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

    protected void CheckIsOnFloor(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.collider.gameObject.layer == Reg.floorLayer ||
                contact.collider.gameObject.layer == Reg.objectLayer)
            {
                if(contact.normal.y == Vector2.up.y)
                {
                    return;
                }
            }
        }

        stateMachine.Trigger(Character.Trigger.Fall);
    }
}
