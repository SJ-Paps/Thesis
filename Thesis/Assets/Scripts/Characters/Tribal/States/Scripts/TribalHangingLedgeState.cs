using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalHangingLedgeState : TribalHSMState
{
    public TribalHangingLedgeState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Jump)
        {
            return TriggerResponse.Reject;
        }

        return TriggerResponse.Accept;
    }
}