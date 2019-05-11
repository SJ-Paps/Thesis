using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOwnable<TOwner> where TOwner : class
{
    TOwner Owner { get; }

    void PropagateOwnerReference(TOwner ownerReference);
}

public interface IBlackboardOwner<TBlackboard>
{
    void PropagateBlackboardReference(TBlackboard blackboard);
}

public interface IBlackboardOwnerExposed<TBlackboard> : IBlackboardOwner<TBlackboard>
{
    TBlackboard Blackboard { get; }
}
