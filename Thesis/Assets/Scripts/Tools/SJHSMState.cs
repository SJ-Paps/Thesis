public abstract class SJHSMState<TState, TTrigger, TOwner, TBlackboard> : HSMState<TState, TTrigger>, IOwnable<TOwner>, IBlackboardOwner<TBlackboard> where TState : unmanaged where TTrigger : unmanaged where TOwner : class
{
    public TOwner Owner { get; protected set; }
    protected TBlackboard Blackboard { get; set; }

    protected bool activeDebug;

    protected SJHSMState(TState stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    public void PropagateOwnerReference(TOwner reference)
    {
        SJHSMState<TState, TTrigger, TOwner, TBlackboard> root = (SJHSMState<TState, TTrigger, TOwner, TBlackboard>)GetRoot();

        root.InternalPropagateOwnerReference(reference);
    }

    private void InternalPropagateOwnerReference(TOwner reference)
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            ((SJHSMState<TState, TTrigger, TOwner, TBlackboard>)parallelChilds[i]).InternalPropagateOwnerReference(reference);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((SJHSMState<TState, TTrigger, TOwner, TBlackboard>)childs[i]).InternalPropagateOwnerReference(reference);
        }

        for (int i = 0; i < transitions.Count; i++)
        {
            foreach (SJGuardCondition<TOwner, TBlackboard> guardCondition in transitions[i])
            {
                guardCondition.PropagateOwnerReference(reference);
            }
        }

        Owner = reference;

        OnOwnerReferencePropagated();
    }

    protected virtual void OnOwnerReferencePropagated()
    {

    }

    public void PropagateBlackboardReference(TBlackboard blackboard)
    {
        SJHSMState<TState, TTrigger, TOwner, TBlackboard> root = (SJHSMState<TState, TTrigger, TOwner, TBlackboard>)GetRoot();

        root.InternalPropagateBlackboardReference(blackboard);
    }

    private void InternalPropagateBlackboardReference(TBlackboard blackboard)
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            ((SJHSMState<TState, TTrigger, TOwner, TBlackboard>)parallelChilds[i]).InternalPropagateBlackboardReference(blackboard);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((SJHSMState<TState, TTrigger, TOwner, TBlackboard>)childs[i]).InternalPropagateBlackboardReference(blackboard);
        }

        for (int i = 0; i < transitions.Count; i++)
        {
            foreach (SJGuardCondition<TOwner, TBlackboard> guardCondition in transitions[i])
            {
                guardCondition.PropagateBlackboardReference(blackboard);
            }
        }

        Blackboard = blackboard;
    }
}

public abstract class SJGuardCondition<TOwner, TBlackboard> : GuardCondition, IOwnable<TOwner>, IBlackboardOwner<TBlackboard> where TOwner : class
{
    public TOwner Owner { get; protected set; }
    protected TBlackboard Blackboard { get; set; }

    public void PropagateOwnerReference(TOwner reference)
    {
        Owner = reference;

        OnOwnerReferencePropagated();
    }

    protected virtual void OnOwnerReferencePropagated()
    {

    }

    public void PropagateBlackboardReference(TBlackboard blackboard)
    {
        Blackboard = blackboard;
    }


}
