using UnityEngine;

public abstract class Weapon : CollectableObject, IActivable, IThrowable {
    
    public virtual bool BeingUsed { get; protected set; }
    
    protected virtual void Awake()
    {

    }

    public void Activate()
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
