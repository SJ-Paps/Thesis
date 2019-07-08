using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalDropingState : TribalHSMState
{
    protected override void OnEnter()
    {
        base.OnEnter();

        IInventoriable obj = Owner.Inventory.Peek(in Tribal.rightHandEquipmentSlotIdentifier);

        if (obj != null && obj.Drop())
        {
            Owner.Equipment.ReleaseSlot(in Tribal.rightHandEquipmentSlotIdentifier);
            Owner.Inventory.RemoveItem(in Tribal.rightHandEquipmentSlotIdentifier);

            Owner.DisplayDropObject(obj as CollectableObject);
        }
        
        SendEvent(Character.Order.FinishAction);
    }
}
