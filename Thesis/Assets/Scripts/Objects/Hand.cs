using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour, IOwnable<IHandOwner>
{
    public CollectableObject Collectable { get; private set; }

    public IHandOwner Owner { get; private set; }

    public bool CollectObject(CollectableObject collectable)
    {
        if(collectable.Collect(Owner))
        {
            Collectable = collectable;

            return true;
        }

        return false;
    }

    public bool DropObject()
    {
        if(Collectable != null && Collectable.Drop())
        {
            Collectable = null;

            return true;
        }

        return false;
    }

    public bool ThrowObject()
    {
        if(Collectable != null && Collectable is IThrowable cached && cached.Throw())
        {
            Collectable = null;

            return true;
        }

        return false;
    }

    public void PropagateOwnerReference(IHandOwner ownerReference)
    {
        Owner = ownerReference;
    }
}
