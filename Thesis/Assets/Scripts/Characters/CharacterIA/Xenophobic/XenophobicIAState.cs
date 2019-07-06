public abstract class XenophobicIAState : IAControllerState
{
    public new XenophobicIAController Owner { get; protected set; }
    protected new Xenophobic Slave { get; set; }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner = (XenophobicIAController)base.Owner;
        Slave = (Xenophobic)base.Slave;
    }

    public bool SendEvent(XenophobicIAController.Trigger trigger)
    {
        return SendEvent((byte)trigger);
    }

    protected sealed override bool HandleEvent(byte trigger)
    {
        return HandleEvent((XenophobicIAController.Trigger)trigger);
    }

    protected virtual bool HandleEvent(XenophobicIAController.Trigger trigger)
    {
        return false;
    }
}
