using SAM.FSM;

public abstract class XenophobicIAState : State<XenophobicIAController.State, XenophobicIAController.Trigger>
{
    protected XenophobicIAController controller;
    protected XenophobicIAController.Blackboard blackboard;

    public XenophobicIAState() : base(null, default(XenophobicIAController.State))
    {
        
    }

    public virtual void InitializeState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard)
    {
        this.controller = controller;
        this.blackboard = blackboard;
        stateMachine = fsm;
        InnerState = state;
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
