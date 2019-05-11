using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalThrowingState : TribalHSMState
{

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.GetHand().ThrowObject();

        SendEvent(Character.Order.FinishAction);
    }
}
