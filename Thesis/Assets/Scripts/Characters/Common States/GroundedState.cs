using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundedState : CharacterState
{
    private Action<Collision2D> checkIsOnFloorDelegate;

    private float contactNormalOffsetY = 0.5f;
    private float contactNormalOffsetX = 0.2f;

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

        EditorDebug.Log("GROUNDED ENTER " + character.name);
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
            if(character.name == "XenophobicEnemy")
            {
                if (contact.collider.gameObject.layer == Reg.floorLayer ||
                contact.collider.gameObject.layer == Reg.objectLayer)
                {
                    Debug.Log(contact.collider.gameObject.layer);
                    Debug.Log(contact.normal.x.ToString("F8"));
                    Debug.Log(contact.normal.y.ToString("F8"));
                }
            }

            if ((contact.collider.gameObject.layer == Reg.floorLayer && contact.normal.y >= contactNormalOffsetY) ||
                (contact.collider.gameObject.layer == Reg.objectLayer && contact.normal.y <= contactNormalOffsetY && contact.normal.x > contactNormalOffsetX))
            {
                return;
            }
        }

        stateMachine.Trigger(Character.Trigger.Fall);
    }
}
