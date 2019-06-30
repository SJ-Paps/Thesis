public class TribalCollectingState : TribalHSMState
{
    private Equipment ownerEquipment;
    private Inventory ownerInventory;

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        ownerEquipment = Configuration.Equipment;
        ownerInventory = Configuration.Inventory;

        ownerEquipment.AddEquipmentSlot(Tribal.rightHandEquipmentSlotIdentifier);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        CollectableObject collectableObject = Blackboard.GetItemOf<IActivable>("Activable") as CollectableObject;

        Blackboard.UpdateItem<IActivable>("Activable", null);

        if (collectableObject != null)
        {
            collectableObject.Collect();

            ownerInventory.AddItem(in Tribal.rightHandEquipmentSlotIdentifier, collectableObject);
            
            Owner.DisplayCollectObject(collectableObject);
            
            if(collectableObject is EquipableObject equipable)
            {
                ownerEquipment.SetObjectAtSlot(in Tribal.rightHandEquipmentSlotIdentifier, equipable);
                Owner.DisplayEquipObject(Configuration.Hand, equipable);
            }
        }

        SendEvent(Character.Order.FinishAction);
    }
}
