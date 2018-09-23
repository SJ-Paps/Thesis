using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class FallingState : CharacterState
{
    private int layerMask = (1 << Reg.floorLayer) | (1 << Reg.objectLayer);

    public FallingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
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
        if(character.CheckIsOnFloor(layerMask))
        {
            stateMachine.Trigger(Character.Trigger.Ground);
        }
    }

    
}
