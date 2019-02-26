public abstract class Weapon : CollectableObject {
    
    public bool BeingUsed { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
    }

    private void Use()
    {
        BeingUsed = true;

        OnUse();
    }

    protected abstract void OnUse();

    protected void FinishUse()
    {
        BeingUsed = false;
    }

    protected override bool ValidateDrop()
    {
        return !BeingUsed;
    }

    public override bool Activate(IHandOwner user)
    {
        Use();

        return true;
    }
}
