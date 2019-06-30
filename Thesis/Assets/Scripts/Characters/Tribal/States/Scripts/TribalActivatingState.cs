using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalActivatingState : TribalHSMState
{
    private Equipment ownerEquipment;

    protected override void OnEnter()
    {
        base.OnEnter();

        ContextualActivable contextualActivable = Blackboard.activable as ContextualActivable;

        Blackboard.activable = null;

        if (contextualActivable != null)
        {
            contextualActivable.Activate(Owner);
        }
        else if(ownerEquipment.HasSomethingEquipped())
        {
            (ownerEquipment.GetEquipable(in Tribal.rightHandEquipmentSlotIdentifier) as CollectableObject).Activate(Owner);
        }

       
        SendEvent(Character.Order.FinishAction);
    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        ownerEquipment = Owner.GetComponentInChildren<Equipment>();
    }
}
