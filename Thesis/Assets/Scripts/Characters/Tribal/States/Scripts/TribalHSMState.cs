public class TribalHSMState : CharacterHSMState
{
    public new Tribal Owner { get; protected set; }
    protected new Blackboard Blackboard { get; private set; }
    

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (Tribal)base.Owner;
        Blackboard = Owner.GetComponent<Blackboard>();
    }
    
    
}