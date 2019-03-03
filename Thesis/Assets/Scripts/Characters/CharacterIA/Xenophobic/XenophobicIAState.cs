public abstract class XenophobicIAState : SJHSMState<XenophobicIAController.State, XenophobicIAController.Trigger>
{
    public new XenophobicIAController Owner { get; protected set; }
    protected new XenophobicIAController.Blackboard Blackboard { get; private set; }

    public XenophobicIAState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (XenophobicIAController)base.Owner;
        Blackboard = (XenophobicIAController.Blackboard)base.Blackboard;
    }
}
