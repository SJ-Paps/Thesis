using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalClimbingLedgeState : TribalClimbingState
{
    public TribalClimbingLedgeState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activeDebug = true;
    }

    protected override void OnEnter()
    {
        base.OnEnter();


    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        SendEvent(Character.Trigger.Fall);
    }
}