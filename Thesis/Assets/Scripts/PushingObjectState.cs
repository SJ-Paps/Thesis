using SAM.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObjectState : CharacterState {

    public Action<Collision2D> checkingForExitingOfPushingObjectMethod;

    public PushingObjectState(
       FSM<Character.State, Character.Trigger> fsm,
       Character.State state,
       Character character,
       List<Character.Order> orders,
       Character.Blackboard blackboard,
       FSM<Character.State, Character.Trigger> jumpingFSM,
       FSM<Character.State, Character.Trigger> movementFSM) : base (fsm, state, character, orders, blackboard)
    {

    }

    protected override void OnEnter() {
        base.OnEnter();
        EditorDebug.Log("PUSHINGOBJECT ENTER");
    }

    protected override void OnExit() {
        base.OnExit();
    }

    protected override void OnUpdate() {
        base.OnUpdate();
    }
}
