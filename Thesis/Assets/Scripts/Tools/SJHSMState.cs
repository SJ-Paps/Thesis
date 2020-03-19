using UnityEngine;
using Paps.StateMachines;
using Paps.StateMachines.HSM;
using SJ;

public abstract class SJHSMState : HSMState<byte, byte>, IOwnable<SJMonoBehaviour>
{
    public SJMonoBehaviour Owner { get; protected set; }

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

        OnConstructionFinished();
    }

    protected virtual void OnConstructionFinished()
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

            Logger.LogConsole("State Transition: " + System.Environment.NewLine + transition.DebugName);
        }
    }
#endif


    protected override void OnEnter()
    {
#if UNITY_EDITOR
        if (activeDebug)
        {
            Logger.LogConsole(DebugName + " ENTER " + Owner);
        }
#endif
    }

    protected void Log(object obj)
    {
#if UNITY_EDITOR
        if (activeDebug)
        {
            Logger.LogConsole(obj);
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
            Logger.LogConsole(DebugName + " EXIT " + Owner);
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

public abstract class SJGuardCondition : GuardCondition, IOwnable<SJMonoBehaviour>
{
    public SJMonoBehaviour Owner { get; protected set; }

    public bool activeDebug;
    public string debugName;

    public void PropagateOwnerReference(SJMonoBehaviour reference)
    {
        Owner = reference;

        OnConstructionFinished();
    }

    protected virtual void OnConstructionFinished()
    {

    }

    protected sealed override bool Validate()
    {
#if UNITY_EDITOR

        bool validated = OnValidate();

        if (activeDebug)
        {
            if (string.IsNullOrEmpty(debugName))
            {
                debugName = GetType().Name;
            }

            Logger.LogConsole(debugName + " Guard Condition Returns " + validated);
        }

        return validated;

#else
        return OnValidate();
#endif
    }

    protected abstract bool OnValidate();


}
