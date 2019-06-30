public abstract class TribalGuardCondition : SJGuardCondition
{
    public new Tribal Owner { get; protected set; }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (Tribal)base.Owner;
    }
}
