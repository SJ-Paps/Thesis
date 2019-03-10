﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalActivatingState : TribalHSMState
{

    protected override void OnEnter()
    {
        base.OnEnter();

        ContextualActivable contextualActivable = Blackboard.activable as ContextualActivable;

        Blackboard.activable = null;

        if (contextualActivable != null)
        {
            contextualActivable.Activate(Owner);
        }
        else if(Owner.GetHand().CurrentCollectable != null)
        {
            Owner.GetHand().ActivateCurrentObject();
        }

       
        SendEvent(Character.Order.FinishAction);
    }
}
