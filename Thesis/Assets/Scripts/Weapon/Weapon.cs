public abstract class Weapon : SJMonoBehaviour {
    
    public bool BeingUsed { get; protected set; }
    
    protected virtual void Awake()
    {
        gameObject.layer = Reg.hostileDeadlyLayer;
    }

    protected abstract void Update();

    public abstract void UseWeapon();
}
