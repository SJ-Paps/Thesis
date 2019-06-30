public abstract class XenophobicIAState : IAControllerHSMState
{
    public new XenophobicIAController Owner { get; protected set; }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner = (XenophobicIAController)base.Owner;
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
