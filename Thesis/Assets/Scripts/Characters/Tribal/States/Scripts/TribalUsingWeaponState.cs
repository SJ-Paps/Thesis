public class TribalUsingWeaponState : TribalHSMState
{
    private Weapon currentWeapon;

    public TribalUsingWeaponState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        if(Owner.GetHand().CurrentCollectable is Weapon weapon)
        {
            currentWeapon = weapon;

            currentWeapon.Activate(Owner);
            Owner.GetHand().UseWeapon();
        }
        else
        {
            SendEvent(Character.Order.FinishAction);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        currentWeapon.Activate(Owner);
        SendEvent(Character.Order.FinishAction);
    }

    protected override void OnExit()
    {
        base.OnExit();

        currentWeapon = null;
    }
}