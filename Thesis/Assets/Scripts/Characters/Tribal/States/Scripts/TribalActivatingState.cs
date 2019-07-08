using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalActivatingState : TribalHSMState
{
    protected override void OnEnter()
    {
        base.OnEnter();

        ContextualActivable contextualActivable = Blackboard.GetItemOf<IActivable>("Activable") as ContextualActivable;

        Blackboard.UpdateItem<IActivable>("Activable", null);

        if (contextualActivable != null)
        {
            contextualActivable.Activate(Owner);
        }
        else if(Owner.Equipment.HasSomethingEquipped())
        {
            (Owner.Equipment.GetEquipable(in Tribal.rightHandEquipmentSlotIdentifier) as CollectableObject).Activate(Owner);
        }

       
        SendEvent(Character.Order.FinishAction);
    }
}
