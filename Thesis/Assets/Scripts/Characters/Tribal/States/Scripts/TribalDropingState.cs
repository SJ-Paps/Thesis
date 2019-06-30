using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalDropingState : TribalHSMState
{
    private Equipment ownerEquipment;
    private Inventory ownerInventory;

    protected override void OnEnter()
    {
        base.OnEnter();

        IInventoriable obj = ownerInventory.Peek(in Tribal.rightHandEquipmentSlotIdentifier);

        if (obj != null && obj.Drop())
        {
            ownerEquipment.ReleaseSlot(in Tribal.rightHandEquipmentSlotIdentifier);
            ownerInventory.RemoveItem(in Tribal.rightHandEquipmentSlotIdentifier);

            Owner.DisplayDropObject(obj as CollectableObject);
        }
        
        SendEvent(Character.Order.FinishAction);
    }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        ownerEquipment = Configuration.Equipment;
        ownerInventory = Configuration.Inventory;
    }
}
