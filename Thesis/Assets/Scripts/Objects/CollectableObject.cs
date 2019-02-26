public abstract class CollectableObject : ActivableObject<IHandOwner>, ICollectable<IHandOwner> {

    public IHandOwner Owner { get; protected set; }

    public bool Collect(IHandOwner user)
    {
        if(Owner == null && ValidateCollect(user))
        {
            PropagateOwnerReference(user);
            OnCollect(user);
            
            return true;
        }

        return false;
    }

    public bool Drop()
    {
        if(Owner != null && ValidateDrop())
        {
            OnDrop();
            ClearOwnerReference();

            return true;
        }

        return false;
    }

    protected virtual bool ValidateCollect(IHandOwner user)
    {
        return true;
    }

    protected virtual bool ValidateDrop()
    {
        return true;
    }

    protected virtual void OnCollect(IHandOwner user)
    {

    }

    protected virtual void OnDrop()
    {

    }

    public void PropagateOwnerReference(IHandOwner ownerReference)
    {
        Owner = ownerReference;
    }

    private void ClearOwnerReference()
    {
        Owner = null;
    }
}
