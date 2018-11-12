using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TribalFallingState : CharacterState
{
    private Animator animator;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orderList, blackboard);

        animator = character.Animator;
    }

    protected override void OnEnter()
    {
        animator.SetTrigger("Fall");

        EditorDebug.Log("FALLING ENTER " + character.name);
    }

    protected override void OnUpdate()
    {
        if(character.IsOnFloor(Reg.walkableLayerMask))
        {
            stateMachine.Trigger(Character.Trigger.Ground);
            return;
        }
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Fall");
    }

    

}
