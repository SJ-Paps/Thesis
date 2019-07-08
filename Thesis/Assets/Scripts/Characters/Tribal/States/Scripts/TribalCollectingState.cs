public class TribalCollectingState : TribalHSMState
{

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner.Equipment.AddEquipmentSlot(Tribal.rightHandEquipmentSlotIdentifier);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        CollectableObject collectableObject = Blackboard.GetItemOf<IActivable>("Activable") as CollectableObject;

        Blackboard.UpdateItem<IActivable>("Activable", null);

        if (collectableObject != null)
        {
            collectableObject.Collect();

            Owner.Inventory.AddItem(in Tribal.rightHandEquipmentSlotIdentifier, collectableObject);
            
            Owner.DisplayCollectObject(collectableObject);
            
            if(collectableObject is EquipableObject equipable)
            {
                Owner.Equipment.SetObjectAtSlot(in Tribal.rightHandEquipmentSlotIdentifier, equipable);
                Owner.DisplayEquipObject(Owner.HandPoint, equipable);
            }
        }

        SendEvent(Character.Order.FinishAction);
    }
}
