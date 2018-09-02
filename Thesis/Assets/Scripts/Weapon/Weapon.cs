public abstract class Weapon : SJMonoBehaviour {
    
    public Character User { get; protected set; }

    public virtual bool BeingUsed { get; protected set; }
    
    protected virtual void Awake()
    {
        gameObject.layer = Reg.hostileDeadlyLayer;
    }

    public void SetUser(Character character)
    {
        User = character;
    }

    public void Drop()
    {
        User = null;
    }

    public void UseWeapon()
    {
        if(User != null && !BeingUsed)
        {
            OnUseWeapon();
        }
    }

    protected abstract void OnUseWeapon();
}
