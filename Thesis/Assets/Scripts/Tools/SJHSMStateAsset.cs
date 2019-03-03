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

    public bool activeDebug;

    public static T BuildFromAsset<T>(TConcreteClass baseAsset, SJMonoBehaviour ownerReference, Blackboard blackboardReference) where T : SJHSMState<TState, TTrigger>
    {
        T baseState = (T)BuildFromAsset(baseAsset);

        baseState.PropagateBlackboardReference(blackboardReference);

        baseState.PropagateOwnerReference(ownerReference);

        return baseState;
    }

    protected override HSMState<TState, TTrigger> CreateConcreteHSMState()
    {
        SJHSMState<TState, TTrigger> state = (SJHSMState<TState, TTrigger>)base.CreateConcreteHSMState();

        state.activeDebug = activeDebug;

        return state;
    }
}
