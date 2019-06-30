public abstract class Weapon : EquipableObject {
    
    public bool InUse { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public bool Use()
    {
        if(Active)
        {
            InUse = true;
            OnUse();
            return true;
        }

        return false;
    }

    public void FinishUse()
    {
        if(InUse)
        {
            InUse = false;
            OnFinishUse();
        }
    }

    protected sealed override bool ValidateDrop()
    {
        return InUse == false && WeaponValidateDrop();
    }

    protected virtual bool WeaponValidateDrop()
    {
        return true;
    }

    protected abstract void OnFinishUse();
    protected abstract void OnUse();
}
