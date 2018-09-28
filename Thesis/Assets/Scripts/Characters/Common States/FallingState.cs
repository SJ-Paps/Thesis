using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;
using System;

public class FallingState : CharacterState
{
    private Action<Collider2D> checkIsOnFloorDelegate;

    private float contactNormalOffsetY = 0.5f;
    private float contactNormalOffsetX = 0.2f;

    private BoxTrigger2D characterFeet;

    public FallingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        checkIsOnFloorDelegate = CheckIsOnFloor;

        characterFeet = character.Feet;
    }

    protected override void OnEnter()
    {
        animator.SetTrigger("Fall");

        characterFeet.onStay += CheckIsOnFloor;

        EditorDebug.Log("FALLING ENTER " + character.name);
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Fall");

        characterFeet.onStay -= CheckIsOnFloor;
    }
    
    private void CheckIsOnFloor(Collider2D collider)
    {
        if (collider.gameObject.layer == Reg.floorLayer || collider.gameObject.layer == Reg.objectLayer)
        {
            stateMachine.Trigger(Character.Trigger.Ground);
        }
    }

}
