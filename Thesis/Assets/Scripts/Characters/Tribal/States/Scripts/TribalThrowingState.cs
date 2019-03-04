using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalThrowingState : TribalHSMState
{
    public TribalThrowingState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.GetHand().ThrowObject();

        SendEvent(Character.Order.FinishAction);
    }
}
