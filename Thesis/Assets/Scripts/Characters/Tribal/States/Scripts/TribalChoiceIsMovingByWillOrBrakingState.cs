﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalChoiceIsMovingByWillOrBrakingState : CharacterHSMState
{
    public TribalChoiceIsMovingByWillOrBrakingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        if(LastEnteringTrigger == Character.Trigger.MoveLeft || LastEnteringTrigger == Character.Trigger.MoveRight)
        {
            SendEvent(Character.Trigger.Move);
        }
        else
        {
            SendEvent(Character.Trigger.StopMoving);
        }
    }
}