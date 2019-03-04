using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalChoiceIsMovingByWillOrBrakingState : TribalHSMState
{
    public TribalChoiceIsMovingByWillOrBrakingState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        if(LastEnteringTrigger == Character.Order.MoveLeft || LastEnteringTrigger == Character.Order.MoveRight)
        {
            SendEvent(Character.Order.Move);
        }
        else
        {
            SendEvent(Character.Order.StopMoving);
        }
    }
}