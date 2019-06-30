using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalActivatingState : TribalHSMState
{
    private Equipment ownerEquipment;

    protected override void OnEnter()
    {
        base.OnEnter();

        ContextualActivable contextualActivable = Blackboard.GetItemOf<IActivable>("Activable") as ContextualActivable;

        Blackboard.UpdateItem<IActivable>("Activable", null);

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

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        ownerEquipment = Configuration.Equipment;
    }
}
