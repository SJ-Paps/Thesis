public class TurretIAControllerHSMState : IAControllerState
{
    public new TurretIAController Owner { get; private set; }
    protected new Turret Slave { get; set; }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner = (TurretIAController)base.Owner;
        Slave = (Turret)base.Slave;
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
