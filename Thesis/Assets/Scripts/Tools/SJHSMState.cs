using UnityEngine;
using Paps.StateMachines;
using Paps.StateMachines.HSM;

public abstract class SJHSMState : HSMState<byte, byte>, IOwnable<SJMonoBehaviour>, IBlackboardOwner<Blackboard>
{
    public SJMonoBehaviour Owner { get; protected set; }
    protected Blackboard Blackboard { get; set; }

    public bool activeDebug;

    protected SJHSMState() : base(0, null)
    {

    }

    public void PropagateOwnerReference(SJMonoBehaviour reference)
    {
        SJHSMState root = (SJHSMState)GetRoot();

        root.InternalPropagateOwnerReference(reference);
    }

    private void InternalPropagateOwnerReference(SJMonoBehaviour reference)
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            ((SJHSMState)parallelChilds[i]).InternalPropagateOwnerReference(reference);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((SJHSMState)childs[i]).InternalPropagateOwnerReference(reference);
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
#if UNITY_EDITOR
        onBeforeAnyTransitionate += DebugTransition;
#endif
    }

#if UNITY_EDITOR
    private void DebugTransition(HSMState<byte, byte> stateFrom, byte trigger, HSMState<byte, byte> stateTo)
    {
        if(activeDebug && (stateFrom == this || stateTo == this))
        {
            HSMTransition<byte, byte> transition = stateFrom.ActiveParent.GetTransition(stateFrom.StateId, trigger);

            EditorDebug.Log("State Transition: " + System.Environment.NewLine + transition.DebugName);
        }
    }
#endif

    public void PropagateBlackboardReference(Blackboard blackboard)
    {
        SJHSMState root = (SJHSMState)GetRoot();

        root.InternalPropagateBlackboardReference(blackboard);
    }

    private void InternalPropagateBlackboardReference(Blackboard blackboard)
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            ((SJHSMState)parallelChilds[i]).InternalPropagateBlackboardReference(blackboard);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((SJHSMState)childs[i]).InternalPropagateBlackboardReference(blackboard);
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

public abstract class SJHSMTransition
{
    [SerializeField]
    public HSMGuardConditionAsset[] ANDGuardConditions, ORGuardConditions;

    public virtual string DebugName { get; }

    public HSMTransition<byte, byte> ToHSMTransition()
    {
        HSMTransition<byte, byte> transition = CreateConcreteTransition();

        for (int i = 0; i < ANDGuardConditions.Length; i++)
        {
            transition.AddANDGuardCondition(ANDGuardConditions[i].CreateConcreteGuardCondition());
        }

        for (int i = 0; i < ORGuardConditions.Length; i++)
        {
            transition.AddORGuardCondition(ORGuardConditions[i].CreateConcreteGuardCondition());
        }

        return transition;
    }

    protected abstract HSMTransition<byte, byte> CreateConcreteTransition();
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
