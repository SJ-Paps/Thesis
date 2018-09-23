using SAM.FSM;

public abstract class XenophobicIAState : State<XenophobicIAController.AlertState, XenophobicIAController.AlertTrigger>
{
    protected XenophobicIAController controller;
    protected XenophobicIAController.Blackboard blackboard;

    public XenophobicIAState(FSM<XenophobicIAController.AlertState, XenophobicIAController.AlertTrigger> fsm, XenophobicIAController.AlertState state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state)
    {
        this.controller = controller;
        this.blackboard = blackboard;
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
