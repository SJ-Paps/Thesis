using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalThrowingState : TribalHSMState
{
    private Equipment ownerEquipment;
    private Inventory ownerInventory;

    protected override void OnEnter()
    {
        base.OnEnter();

        if(ownerEquipment.HasEquippedObjectOfType<IThrowable>(out IThrowable throwable))
        {
            throwable.Throw();
        }

        SendEvent(Character.Order.FinishAction);
    }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        ownerEquipment = Owner.GetComponentInChildren<Equipment>();
        ownerInventory = Owner.GetComponentInChildren<Inventory>();
    }
}
