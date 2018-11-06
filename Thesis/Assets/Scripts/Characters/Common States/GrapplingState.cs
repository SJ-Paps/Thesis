using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingState : CharacterState 
{
    FSM<Character.State, Character.Trigger> characterMovementFSM;
    FSM<Character.State, Character.Trigger> characterActionFSM;

    public GrapplingState(
        FSM<Character.State, Character.Trigger> fsm, 
        Character.State state, 
        Character character, 
        List<Character.Order> orderList, 
        FSM<Character.State, Character.Trigger> movementFSM, 
        FSM<Character.State, Character.Trigger> actionFSM,
        Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        characterMovementFSM = movementFSM;
        characterActionFSM = actionFSM;
    }

    protected override void OnEnter() 
    {
        EditorDebug.Log("GRAPPLING ENTER");
        characterMovementFSM.Active = false;
        characterActionFSM.Active = false;
    }

    protected override void OnUpdate() 
    {
        Grappled();
    }

    protected override void OnExit() 
    {
        base.OnExit();
    }

    private void Grappled()
    {
        character.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
}
