using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOwnable<TOwner> where TOwner : class
{
    TOwner Owner { get; }

    void PropagateOwnerReference(TOwner ownerReference);
}
