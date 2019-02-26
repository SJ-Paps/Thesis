public abstract class CollectableObject : ActivableObject<IHandOwner>, ICollectable<IHandOwner> {

    public IHandOwner Owner { get; protected set; }

    public virtual bool Collect(IHandOwner user)
    {
        if(ValidateCollect())
        {
            PropagateOwnerReference(user);

            return true;
        }

        return false;
    }

    public virtual bool Drop()
    {
        if(ValidateDrop())
        {
            ClearOwnerReference();

            return true;
        }

        return false;
    }

    protected virtual bool ValidateCollect()
    {
        return true;
    }

    protected virtual bool ValidateDrop()
    {
        return true;
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
