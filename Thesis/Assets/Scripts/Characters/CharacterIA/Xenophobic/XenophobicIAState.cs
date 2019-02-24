public abstract class XenophobicIAState : SJHSMState<XenophobicIAController.State, XenophobicIAController.Trigger, XenophobicIAController, XenophobicIAController.Blackboard>
{

    public XenophobicIAState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {

    }
}
