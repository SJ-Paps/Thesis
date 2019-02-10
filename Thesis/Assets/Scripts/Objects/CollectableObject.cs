public abstract class CollectableObject : ActivableObject, ICollectable<Character> {

    public Character Owner { get; protected set; }

    public virtual bool Collect(Character user)
    {
        Owner = user;

        return true;
    }

    public virtual bool Drop()
    {
        Owner = null;

        return true;
    }

    public void PropagateOwnerReference(Character ownerReference)
    {
        
    }
}
