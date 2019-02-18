using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAController<TSlave, TState, TTrigger, TBlackboard> : UnityController<TSlave, Character.Trigger> 
                                                                            where TSlave : Character
                                                                            where TState : unmanaged
                                                                            where TTrigger : unmanaged
{

    private IAControllerHSMState<TSlave, TState, TTrigger, IAController<TSlave, TState, TTrigger, TBlackboard>, TBlackboard> hsm;
    
}

public class IAControllerHSMState<TSlave, TState, TTrigger, TOwner, TBlackboard> : SJHSMState<TState, TTrigger, TOwner, TBlackboard> 
                                                                                where TSlave : Character 
                                                                                where TOwner : IAController<TSlave, TState, TTrigger, TBlackboard> 
                                                                                where TState : unmanaged
                                                                                where TTrigger : unmanaged
{
    protected IAControllerHSMState(TState stateId, string debugName = null) : base(stateId, debugName)
    {

    }
}
