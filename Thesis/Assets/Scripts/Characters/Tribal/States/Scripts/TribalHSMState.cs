public class TribalHSMState : CharacterHSMState<Tribal.State>
{
    public new Tribal Owner { get; protected set; }
    protected new Tribal.Blackboard Blackboard { get; private set; }

    public TribalHSMState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (Tribal)base.Owner;
        Blackboard = (Tribal.Blackboard)base.Blackboard;
    }
}