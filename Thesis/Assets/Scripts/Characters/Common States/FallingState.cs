using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;
using System;

public class FallingState : CharacterState
{
    private Action<Collision2D> checkIsOnFloorDelegate;

    public FallingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        checkIsOnFloorDelegate = CheckIsOnFloor;
    }

    protected override void OnEnter()
    {
        animator.SetTrigger("Fall");

        character.onCollisionStay2D += CheckIsOnFloor;

        EditorDebug.Log("FALLING ENTER");
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Fall");

        character.onCollisionStay2D -= CheckIsOnFloor;
    }


    protected void CheckIsOnFloor(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.collider.gameObject.layer == Reg.floorLayer ||
                contact.collider.gameObject.layer == Reg.objectLayer)
            {
                if (contact.normal.y == Vector2.up.y)
                {
                    stateMachine.Trigger(Character.Trigger.Ground);
                }
            }
        }
    }

}
