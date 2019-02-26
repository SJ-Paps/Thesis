public class TribalUsingWeaponState : TribalHSMState
{
    private Weapon currentWeapon;

    public TribalUsingWeaponState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        if(Owner.GetHand().CurrentCollectable is Weapon cached)
        {
            currentWeapon = cached;

            Owner.GetHand().ActivateCurrentObject();
        }
        else
        {
            SendEvent(Character.Trigger.StopAttacking);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(currentWeapon.BeingUsed == false)
        {
            SendEvent(Character.Trigger.StopAttacking);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        currentWeapon = null;
    }
}