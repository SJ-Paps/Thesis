using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;
using System;

public class FallingState : CharacterState
{
    private Action<Collision2D> checkIsOnFloorDelegate;

    private float contactNormalOffsetY = 0.5f;
    private float contactNormalOffsetX = 0.2f;

    public FallingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        checkIsOnFloorDelegate = CheckIsOnFloor;
    }

    protected override void OnEnter()
    {
        animator.SetTrigger("Fall");

        character.onCollisionStay2D += CheckIsOnFloor;

        EditorDebug.Log("FALLING ENTER " + character.name);
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
            /*if (character.name == "MainCharacter")
            {
                if (contact.collider.gameObject.layer == Reg.floorLayer ||
                contact.collider.gameObject.layer == Reg.objectLayer)
                {
                    Debug.Log(contact.collider.gameObject.layer);
                    Debug.Log(contact.normal.x.ToString("F8"));
                    Debug.Log(contact.normal.y.ToString("F8"));
                }
            }*/

            if ((contact.collider.gameObject.layer == Reg.floorLayer && contact.normal.y >= contactNormalOffsetY) ||
                (contact.collider.gameObject.layer == Reg.objectLayer && contact.normal.y >= contactNormalOffsetY && contact.normal.x < contactNormalOffsetX))
            {
                stateMachine.Trigger(Character.Trigger.Ground);
            }
        }
    }

}
