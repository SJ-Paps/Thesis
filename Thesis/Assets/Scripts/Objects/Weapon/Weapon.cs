using UnityEngine;

public abstract class Weapon : CollectableObject, IThrowable {
    
    public virtual bool BeingUsed { get; protected set; }
    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Activate(IHandOwner user)
    {
        if(Owner != null && !BeingUsed)
        {
            OnUseWeapon();
        }
    }

    public override bool Drop()
    {
        if(!BeingUsed)
        {
            return base.Drop();
        }

        return false;
    }

    protected abstract void OnUseWeapon();
    public abstract bool Throw();

    public override bool ShouldBeSaved()
    {
        return true;
    }
}
