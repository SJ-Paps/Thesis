using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalChoiceWalkingOrTrottingOrRunningState : TribalHSMState
{
    public TribalChoiceWalkingOrTrottingOrRunningState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        SendEvent(Character.Trigger.Trot);
    }
}