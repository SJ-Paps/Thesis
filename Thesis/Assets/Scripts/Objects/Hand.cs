using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour, IOwnable<IHandOwner>
{
    public CollectableObject Collectable { get; private set; }

    public IHandOwner Owner { get; private set; }

    public void CollectObject(CollectableObject collectable)
    {
        if(collectable.Collect(Owner))
        {
            Collectable = collectable;
        }
    }

    public void DropObject()
    {
        if(Collectable != null)
        {
            Collectable.Drop();
        }
    }

    public void ThrowObject()
    {
        if(Collectable != null && Collectable is IThrowable cached)
        {
            cached.Throw();
        }
    }

    public void PropagateOwnerReference(IHandOwner ownerReference)
    {
        Owner = ownerReference;
    }
}
