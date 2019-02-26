using UnityEngine;

public class Hand : SJMonoBehaviour, IOwnable<IHandOwner>
{
    public CollectableObject CurrentCollectable { get; private set; }

    public IHandOwner Owner { get; private set; }

    public bool IsFree
    {
        get
        {
            return CurrentCollectable == null;
        }
    }

    public bool CollectObject(CollectableObject collectable)
    {
        if (IsFree && collectable.Collect(Owner))
        {
            CurrentCollectable = collectable;

            return true;
        }

        return false;
    }

    public bool DropObject()
    {
        if (!IsFree && CurrentCollectable.Drop())
        {
            CurrentCollectable = null;

            return true;
        }

        return false;
    }

    public bool ThrowObject()
    {
        if (!IsFree && CurrentCollectable is IThrowable throwable && throwable.Throw())
        {
            CurrentCollectable = null;

            return true;
        }

        return false;
    }

    public bool ActivateCurrentObject()
    {
        if (!IsFree)
        {
            return CurrentCollectable.Activate(Owner);
        }

        return false;
    }

    public bool ActivateObject(ActivableObject<IHandOwner> activable)
    {
        return activable.Activate(Owner);
    }

    public void PropagateOwnerReference(IHandOwner ownerReference)
    {
        Owner = ownerReference;
    }
}
