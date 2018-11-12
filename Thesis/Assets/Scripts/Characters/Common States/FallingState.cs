using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : CharacterState
{
    private float contactNormalOffsetY = 0.5f;
    private float contactNormalOffsetX = 0.2f;

    private int floorLayers = (1 << Reg.floorLayer) | (1 << Reg.objectLayer);

    private Animator animator;

    public FallingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        animator = character.Animator;
    }

    protected override void OnEnter()
    {
        animator.SetTrigger("Fall");

        EditorDebug.Log("FALLING ENTER " + character.name);
    }

    protected override void OnUpdate()
    {
        if(character.IsOnFloor(floorLayers))
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
