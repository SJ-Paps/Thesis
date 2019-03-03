using UnityEngine;

public abstract class SJHSMState<TState, TTrigger> : HSMState<TState, TTrigger>, IOwnable<SJMonoBehaviour>, IBlackboardOwner<Blackboard> where TState : unmanaged where TTrigger : unmanaged
{
    public SJMonoBehaviour Owner { get; protected set; }
    protected Blackboard Blackboard { get; set; }

    public bool activeDebug;

    protected SJHSMState(TState stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    public void PropagateOwnerReference(SJMonoBehaviour reference)
    {
        SJHSMState<TState, TTrigger> root = (SJHSMState<TState, TTrigger>)GetRoot();

        root.InternalPropagateOwnerReference(reference);
    }

    private void InternalPropagateOwnerReference(SJMonoBehaviour reference)
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            ((SJHSMState<TState, TTrigger>)parallelChilds[i]).InternalPropagateOwnerReference(reference);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((SJHSMState<TState, TTrigger>)childs[i]).InternalPropagateOwnerReference(reference);
        }

        for (int i = 0; i < transitions.Count; i++)
        {
            foreach (SJGuardCondition guardCondition in transitions[i])
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

    public void PropagateBlackboardReference(Blackboard blackboard)
    {
        SJHSMState<TState, TTrigger> root = (SJHSMState<TState, TTrigger>)GetRoot();

        root.InternalPropagateBlackboardReference(blackboard);
    }

    private void InternalPropagateBlackboardReference(Blackboard blackboard)
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            ((SJHSMState<TState, TTrigger>)parallelChilds[i]).InternalPropagateBlackboardReference(blackboard);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((SJHSMState<TState, TTrigger>)childs[i]).InternalPropagateBlackboardReference(blackboard);
        }

        for (int i = 0; i < transitions.Count; i++)
        {
            foreach (SJGuardCondition guardCondition in transitions[i])
            {
                guardCondition.PropagateBlackboardReference(blackboard);
            }
        }

        Blackboard = blackboard;
    }


    protected override void OnEnter()
    {
#if UNITY_EDITOR
        if (activeDebug)
        {
            EditorDebug.Log(DebugName + " ENTER " + Owner);
        }
#endif
    }

    protected void Log(object obj)
    {
#if UNITY_EDITOR
        if (activeDebug)
        {
            EditorDebug.Log(obj);
        }
#endif
    }


    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
#if UNITY_EDITOR
        if (activeDebug)
        {
            EditorDebug.Log(DebugName + " EXIT " + Owner);
        }
#endif
    }
}

public abstract class SJGuardCondition : GuardCondition, IOwnable<SJMonoBehaviour>, IBlackboardOwner<Blackboard>
{
    public SJMonoBehaviour Owner { get; protected set; }
    protected Blackboard Blackboard { get; set; }

    public bool activeDebug;
    public string debugName;

    public void PropagateOwnerReference(SJMonoBehaviour reference)
    {
        Owner = reference;

        OnOwnerReferencePropagated();
    }

    protected virtual void OnOwnerReferencePropagated()
    {

    }

    protected sealed override bool Validate()
    {
#if UNITY_EDITOR

        bool validated = OnValidate();

        if (activeDebug)
        {
            EditorDebug.Log(debugName + " Guard Condition Returns " + validated);
        }

        return validated;

#else
        return OnValidate();
#endif
    }

    protected abstract bool OnValidate();

    public void PropagateBlackboardReference(Blackboard blackboard)
    {
        Blackboard = blackboard;
    }


}
