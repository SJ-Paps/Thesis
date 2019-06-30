public class TurretIAControllerHSMState : SJHSMState
{
    public new TurretIAController Owner { get; private set; }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (TurretIAController)base.Owner;
    }

    public bool SendEvent(TurretIAController.Trigger trigger)
    {
        return SendEvent((byte)trigger);
    }

    protected sealed override bool HandleEvent(byte trigger)
    {
        return HandleEvent((TurretIAController.Trigger)trigger);
    }

    protected virtual bool HandleEvent(TurretIAController.Trigger trigger)
    {
        return false;
    }
}
