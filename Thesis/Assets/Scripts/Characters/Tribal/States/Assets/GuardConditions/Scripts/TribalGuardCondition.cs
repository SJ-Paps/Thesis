public abstract class TribalGuardCondition : CharacterGuardCondition
{
    public new Tribal Owner { get; protected set; }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner = (Tribal)base.Owner;
    }
}
