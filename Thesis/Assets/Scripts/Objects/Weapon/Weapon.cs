public abstract class Weapon : CollectableObject {
    
    public virtual bool BeingUsed { get; protected set; }
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override bool ValidateDrop()
    {
        return !BeingUsed;
    }

    public override bool ShouldBeSaved()
    {
        return true;
    }
}
