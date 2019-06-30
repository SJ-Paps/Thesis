public class TribalUsingWeaponState : TribalHSMState
{
    private Equipment ownerEquipment;
    private Inventory ownerInventory;

    private Weapon currentInUseWeapon;

    protected override void OnEnter()
    {
        base.OnEnter();

        if(ownerEquipment.HasEquippedObjectOfType<Weapon>(out Weapon weapon))
        {
            if(weapon.Use() == false)
            {
                SendEvent(Character.Order.FinishAction);
            }
            else
            {
                currentInUseWeapon = weapon;
            }
        }
        else
        {
            SendEvent(Character.Order.FinishAction);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        currentInUseWeapon.FinishUse();

        SendEvent(Character.Order.FinishAction);
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        ownerEquipment = Configuration.Equipment;
        ownerInventory = Configuration.Inventory;
    }
}