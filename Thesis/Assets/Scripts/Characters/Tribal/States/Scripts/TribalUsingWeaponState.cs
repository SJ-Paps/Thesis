public class TribalUsingWeaponState : TribalHSMState
{
    private Weapon currentInUseWeapon;

    protected override void OnEnter()
    {
        base.OnEnter();

        if(Owner.Equipment.HasEquippedObjectOfType<Weapon>(out Weapon weapon))
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
}