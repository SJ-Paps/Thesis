using UnityEngine;

public abstract class Weapon : CollectableObject, IActivable<Character>, IThrowable {
    
    public virtual bool BeingUsed { get; protected set; }
    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Activate(Character user)
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
