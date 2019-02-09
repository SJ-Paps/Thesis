public abstract class XenophobicIAState : HSMState<XenophobicIAController.State, XenophobicIAController.Trigger>
{
    protected XenophobicIAController controller;
    protected XenophobicIAController.Blackboard blackboard;

    public XenophobicIAState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnEnter()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnExit()
    {
        
    }
}
