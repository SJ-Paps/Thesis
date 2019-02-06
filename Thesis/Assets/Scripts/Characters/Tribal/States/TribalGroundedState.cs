using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TribalGroundedState : TribalHSMState
{

    public TribalGroundedState(Character.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        EditorDebug.Log("GROUNDED ENTER " + character.name);
    }

    protected override void OnExit() {
        base.OnExit();

        EditorDebug.Log("GROUNDED EXIT " + character.name);
    }
}
