public abstract class Weapon : CollectableObject {
    
    protected override void Awake()
    {
        base.Awake();
    }

    public bool Use()
    {
        if(State == ActivableState.On)
        {
            OnUse();
            return true;
        }

        return false;
    }

    protected abstract void OnUse();
}
