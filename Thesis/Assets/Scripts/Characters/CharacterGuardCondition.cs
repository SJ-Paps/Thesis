public abstract class CharacterGuardCondition : SJGuardCondition
{
    protected Blackboard Blackboard { get; set; }
    public new Character Owner { get; protected set; }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner = (Character)base.Owner;
        Blackboard = Owner.Blackboard;
    }
}
