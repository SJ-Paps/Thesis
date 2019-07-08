public class TribalHSMState : CharacterHSMState
{
    public new Tribal Owner { get; protected set; }
    protected new Blackboard Blackboard { get; private set; }
    

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner = (Tribal)base.Owner;
        Blackboard = Owner.Blackboard;
    }
    
    
}