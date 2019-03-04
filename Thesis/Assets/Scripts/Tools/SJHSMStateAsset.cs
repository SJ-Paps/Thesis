using UnityEngine;

public abstract class SJHSMStateAsset : HSMStateAsset<byte, byte>
{
    [SerializeField]
    private SJHSMStateAsset[] childs;

    [SerializeField]
    private SJHSMStateAsset[] parallelChilds;

    public bool activeDebug;

    public static T BuildFromAsset<T>(SJHSMStateAsset baseAsset, SJMonoBehaviour ownerReference, Blackboard blackboardReference) where T : SJHSMState
    {
        T baseState = (T)BuildFromAsset(baseAsset);

        baseState.PropagateBlackboardReference(blackboardReference);

        baseState.PropagateOwnerReference(ownerReference);

        return baseState;
    }

    protected override HSMState<byte, byte> CreateConcreteHSMState()
    {
        SJHSMState state = (SJHSMState)base.CreateConcreteHSMState();

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
