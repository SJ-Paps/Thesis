using UnityEngine;
using System;
using Paps.StateMachines.HSM;

public abstract class SJHSMStateAsset : HSMStateAsset<byte, byte>
{
    [SerializeField]
    private SJHSMStateAsset[] childs;

    public SJHSMStateAsset[] Childs
    {
        get
        {
            return childs;
        }
    }

    [SerializeField]
    private SJHSMStateAsset[] parallelChilds;

    public SJHSMStateAsset[] ParallelChilds
    {
        get
        {
            return parallelChilds;
        }
    }

    public bool activeDebug;

    public static T BuildFromAsset<T>(SJHSMStateAsset baseAsset, SJMonoBehaviour ownerReference, IConfiguration configuration) where T : SJHSMState
    {
        T baseState = (T)BuildFromAsset(baseAsset);

        baseState.PropagateConfigurationReference(configuration);

        baseState.PropagateOwnerReference(ownerReference);

        return baseState;
    }

    protected override HSMState<byte, byte> CreateConcreteHSMState()
    {
        SJHSMState state = (SJHSMState)Activator.CreateInstance(Type.GetType(stateClassFullName));

        state.ChangeStateId(stateId);
        state.DebugName = debugName;

        state.activeDebug = activeDebug;

        return state;
    }

    protected sealed override HSMTransition<byte, byte>[] GetTransitions()
    {
        SJHSMTransition[] sjtransitions = GetSJHSMTranstions();
        HSMTransition<byte, byte>[] transitions = new HSMTransition<byte, byte>[sjtransitions.Length];


        for (int i = 0; i < sjtransitions.Length; i++)
        {
            transitions[i] = sjtransitions[i].ToHSMTransition();
        }

        return transitions;
    }

    protected abstract SJHSMTransition[] GetSJHSMTranstions();

    protected override HSMStateAsset<byte, byte>[] GetNonParallelChilds()
    {
        return childs;
    }

    protected override HSMStateAsset<byte, byte>[] GetParallelChilds()
    {
        return parallelChilds;
    }

}

public abstract class SJHSMTransition<TState, TTrigger> : SJHSMTransition where TState : Enum where TTrigger : Enum
{
    [SerializeField]
    private string titleName;

    [SerializeField]
    private bool disabled;

    [SerializeField]
    private TState stateFrom;

    [SerializeField]
    private TTrigger trigger;

    [SerializeField]
    private TState stateTo;
    
    private string debugName;

    public override string DebugName
    {
        get
        {
            if(string.IsNullOrEmpty(debugName))
            {
                debugName = titleName + ' ' + "State From: " + stateFrom.ToString() + " Trigger: " + trigger.ToString() + " State To: " + stateTo.ToString();
            }

            return debugName;
        }
    }

    protected override HSMTransition<byte, byte> CreateConcreteTransition()
    {
        HSMTransition<byte, byte> transition = new HSMTransition<byte, byte>((byte)(object)stateFrom, (byte)(object)trigger, (byte)(object)stateTo);

        transition.disabled = disabled;
        transition.DebugName = DebugName;

        return transition;
    }
}

public abstract class SJHSMStateAsset<TState, TTrigger, TConcreteHSMTransitionClass> : SJHSMStateAsset where TState : Enum where TTrigger : Enum where TConcreteHSMTransitionClass : SJHSMTransition<TState, TTrigger>
{
    [SerializeField]
    private TState state;

    [SerializeField]
    private TConcreteHSMTransitionClass[] transitions;

    protected override SJHSMTransition[] GetSJHSMTranstions()
    {
        return transitions;
    }

    protected override byte GetStateId()
    {
        return (byte)(object)state;
    }
}
