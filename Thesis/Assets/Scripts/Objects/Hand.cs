using UnityEngine;

public class Hand : SJMonoBehaviour, IOwnable<IHandOwner>
{
    public IActivable<IHandOwner> CurrentActivable { get; private set; }

    public IHandOwner Owner { get; private set; }

    public bool IsFree
    {
        get
        {
            return CurrentActivable == null;
        }
    }

    public bool CollectObject(CollectableObject collectable)
    {
        if (IsFree && collectable.Collect(Owner))
        {
            CurrentActivable = collectable;

            return true;
        }

        return false;
    }

    public bool DropObject()
    {
        if (!IsFree && CurrentActivable is ICollectable<IHandOwner> cached && cached.Drop())
        {
            CurrentActivable = null;

            return true;
        }

        return false;
    }

    public bool ThrowObject()
    {
        if (!IsFree && CurrentActivable is IThrowable cached && cached.Throw())
        {
            CurrentActivable = null;

            return true;
        }

        return false;
    }

    public bool ActivateCurrentObject()
    {
        if (!IsFree)
        {
            return CurrentActivable.Activate(Owner);
        }

        return false;
    }

    public bool ActivateObject(IActivable<IHandOwner> activable)
    {
        return activable.Activate(Owner);
    }

    public void PropagateOwnerReference(IHandOwner ownerReference)
    {
        Owner = ownerReference;
    }
}
