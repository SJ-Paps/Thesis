public class TribalUsingWeaponState : TribalHSMState
{
    public TribalUsingWeaponState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        if(Owner.GetHand().CurrentCollectable is Weapon weapon)
        {
            if(Owner.GetHand().UseWeapon() == false)
            {
                SendEvent(Character.Order.FinishAction);
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

        Owner.GetHand().FinishUseWeapon();

        SendEvent(Character.Order.FinishAction);
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}