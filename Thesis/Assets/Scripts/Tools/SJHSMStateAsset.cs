using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SJHSMStateAsset<TConcreteClass, THSMTransitionWrapper, TState, TTrigger, TOwner, TBlackboard> : HSMStateAsset<TConcreteClass, THSMTransitionWrapper, TState, TTrigger>
                                                                                                            where TState : unmanaged
                                                                                                            where TTrigger : unmanaged
                                                                                                            where TOwner : class
                                                                                                            where TConcreteClass : SJHSMStateAsset<TConcreteClass, THSMTransitionWrapper, TState, TTrigger, TOwner, TBlackboard>
                                                                                                            where THSMTransitionWrapper : IHSMTransitionSerializationWrapper<TState, TTrigger>
{
    public static T BuildFromAsset<T>(TConcreteClass baseAsset, TOwner ownerReference, TBlackboard blackboardReference) where T : SJHSMState<TState, TTrigger, TOwner, TBlackboard>
    {
        T baseState = (T)BuildFromAsset(baseAsset);

        baseState.PropagateOwnerReference(ownerReference);

        baseState.PropagateBlackboardReference(blackboardReference);

        return baseState;
    }
}
