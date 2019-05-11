public abstract class XenophobicIAState : SJHSMState
{
    public new XenophobicIAController Owner { get; protected set; }
    protected new XenophobicIAController.Blackboard Blackboard { get; private set; }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (XenophobicIAController)base.Owner;
        Blackboard = (XenophobicIAController.Blackboard)base.Blackboard;
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
