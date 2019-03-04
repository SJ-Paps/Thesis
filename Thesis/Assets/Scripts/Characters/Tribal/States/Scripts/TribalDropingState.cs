using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalDropingState : TribalHSMState
{
    public TribalDropingState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.GetHand().DropObject();

        SendEvent(Character.Order.FinishAction);
    }
}
