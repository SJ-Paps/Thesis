using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalDropingState : TribalHSMState
{

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.GetHand().DropObject();

        SendEvent(Character.Order.FinishAction);
    }
}
