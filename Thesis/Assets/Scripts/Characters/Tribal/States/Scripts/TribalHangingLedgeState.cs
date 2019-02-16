using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalHangingLedgeState : TribalHSMState
{
    public TribalHangingLedgeState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Jump)
        {
            return true;
        }

        return false;
    }
}