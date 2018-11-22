using UnityEngine;

public abstract class Weapon : CollectableObject, IActivable, IThrowable {
    
    public virtual bool BeingUsed { get; protected set; }
    
    protected override void Awake()
    {
        base.Awake();
    }

    public void Activate(Character user)
    {
        if(User != null && !BeingUsed)
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
}
