using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class FallingState : CharacterState
{
    public FallingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {

    }

    protected override void OnEnter()
    {
        EditorDebug.Log("FALLING ENTER");
        blackboard.isFalling = true;
    }

    protected override void OnExit()
    {
        base.OnExit();
        blackboard.isFalling = false;
    }

    protected override void OnUpdate()
    {
        if(character.CheckIsOnFloor())
        {
            stateMachine.Trigger(Character.Trigger.Ground);
        }
    }

    
}
