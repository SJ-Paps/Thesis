using SAM.FSM;

public abstract class XenophobicIAState : State<XenophobicIAController.State, XenophobicIAController.Trigger>
{
    protected Xenophobic character;
    protected XenophobicIAController.Blackboard blackboard;

    public XenophobicIAState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, Xenophobic controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state)
    {
        character = controller;
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
