using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class FallingState : CharacterState
{
    public FallingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList) : base(fsm, state, character, orderList)
    {
    }

    protected override void OnEnter()
    {
        EditorDebug.Log("FALLING ENTER");
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnUpdate()
    {
        if(character.CheckIsOnFloor())
        {
            stateMachine.Trigger(Character.Trigger.Ground);
        }
    }

    
}
