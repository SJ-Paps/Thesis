using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalActivatingState : TribalHSMState
{
    public TribalActivatingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        if(Blackboard.CurrentFrameActivables.ContainsType<ContextualActivable>(out ContextualActivable contextualActivable))
        {
            contextualActivable.Activate(Owner);
        }
        else if(Owner.GetHand().CurrentCollectable != null)
        {
            if(Owner.GetHand().CurrentCollectable is Weapon)
            {
                SendEvent(Character.Order.Attack);
            }
            else
            {
                Owner.GetHand().ActivateCurrentObject();
                SendEvent(Character.Order.FinishAction);
            }
        }
    }
}
