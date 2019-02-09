using System;
using System.Collections;
using System.Collections.Generic;

public enum TriggerResponse : byte
{
    Accept,
    Reject
}

public delegate void StateChanged<TState, TTrigger>(TState previous, TState current, TTrigger trigger) where TState : unmanaged where TTrigger : unmanaged;

public abstract class HSMState<TState, TTrigger> where TState : unmanaged where TTrigger : unmanaged
{
    public string DebugName { get; protected set; }

    public TState StateId { get; protected set; }

    protected List<HSMState<TState, TTrigger>> parents;
    protected List<HSMState<TState, TTrigger>> childs;
    protected List<HSMState<TState, TTrigger>> parallelChilds;

    protected List<HSMTransition<TState, TTrigger>> transitions;

    public TState InitialChildState { get; private set; }

    public HSMState<TState, TTrigger> ActiveParent { get; protected set; }
    public HSMState<TState, TTrigger> ActiveNonParallelChild { get; protected set; }

    protected Func<TState, TState, bool> stateIdComparer;
    protected Func<TTrigger, TTrigger, bool> triggerComparer;

    public virtual int ChildCount
    {
        get
        {
            return childs.Count + parallelChilds.Count;
        }
    }

    public event StateChanged<TState, TTrigger> onStateChanged;
    public event StateChanged<TState, TTrigger> onAnyStateChanged
    {
        add
        {
            GetRoot().AddOnAnyStateChangedListener(value);
        }

        remove
        {
            GetRoot().RemoveOnAnyStateChangedListener(value);
        }
    }

    protected HSMState(TState stateId, string debugName = null, Func<TState, TState, bool> stateIdComparer = null, Func<TTrigger, TTrigger, bool> triggerComparer = null)
    {
        StateId = stateId;

        childs = new List<HSMState<TState, TTrigger>>();
        parallelChilds = new List<HSMState<TState, TTrigger>>();
        parents = new List<HSMState<TState, TTrigger>>();
        transitions = new List<HSMTransition<TState, TTrigger>>();

        if (debugName != null)
        {
            DebugName = debugName;
        }
        else
        {
            DebugName = GetType().Name;
        }

        if(stateIdComparer != null)
        {
            this.stateIdComparer = stateIdComparer;
        }
        else
        {
            this.stateIdComparer = DefaultComparer<TState>;
        }

        if(triggerComparer != null)
        {
            this.triggerComparer = triggerComparer;
        }
        else
        {
            this.triggerComparer = DefaultComparer<TTrigger>;
        }
    }

    private void SetActiveParent(HSMState<TState, TTrigger> parent)
    {
        ActiveParent = parent;
    }

    public void Enter()
    {
        OnEnter();

        EnterParallelChilds();

        ActiveNonParallelChild = GetImmediateChildState(InitialChildState);

        EnterNonParallelActiveChild();
    }

    private void EnterParallelChilds()
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            var current = parallelChilds[i];

            current.SetActiveParent(this);
            current.Enter();
        }
    }

    private void EnterNonParallelActiveChild()
    {
        if (ActiveNonParallelChild != null)
        {
            ActiveNonParallelChild.SetActiveParent(this);
            ActiveNonParallelChild.Enter();
        }
    }

    public virtual void Update()
    {
        UpdateNonParallelActiveChild();

        UpdateParallelChilds();

        OnUpdate();
    }

    private void UpdateNonParallelActiveChild()
    {
        if (ActiveNonParallelChild != null)
        {
            ActiveNonParallelChild.Update();
        }
    }

    private void UpdateParallelChilds()
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            parallelChilds[i].Update();
        }
    }

    public void Exit()
    {
        ExitActiveNonParallelChild();

        ExitParallelChilds();

        OnExit();
    }

    private void ExitParallelChilds()
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            var current = parallelChilds[i];

            current.SetActiveParent(null);
            current.Exit();
        }
    }

    private void ExitActiveNonParallelChild()
    {
        if (ActiveNonParallelChild != null)
        {
            ActiveNonParallelChild.Exit();
            ActiveNonParallelChild.SetActiveParent(null);
            ActiveNonParallelChild = null;
        }
    }

    protected virtual void OnEnter()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnExit()
    {

    }

    private void AddOnAnyStateChangedListener(StateChanged<TState, TTrigger> listener)
    {
        for(int i = 0; i < parallelChilds.Count; i++)
        {
            parallelChilds[i].AddOnAnyStateChangedListener(listener);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].AddOnAnyStateChangedListener(listener);
        }

        onStateChanged += listener;
    }

    private void RemoveOnAnyStateChangedListener(StateChanged<TState, TTrigger> listener)
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            parallelChilds[i].RemoveOnAnyStateChangedListener(listener);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].RemoveOnAnyStateChangedListener(listener);
        }

        onStateChanged -= listener;
    }

    public void AddChild(HSMState<TState, TTrigger> child)
    {
        if(ContainsImmediateChildState(child.StateId) == false)
        {
            childs.Add(child);
            child.AddParent(this);
        }
    }

    public void AddParallelChild(HSMState<TState, TTrigger> child)
    {
        if(ContainsImmediateChildState(child.StateId) == false)
        {
            parallelChilds.Add(child);
            child.AddParent(this);
        }
    }

    private void AddParent(HSMState<TState, TTrigger> parent)
    {
        parents.Add(parent);
    }

    public void RemoveChild(TState stateId)
    {
        HSMState<TState, TTrigger> toRemove = GetImmediateChildState(stateId);

        if (childs.Remove(toRemove))
        {
            BreakAllTransitionFrom(stateId);
            toRemove.RemoveParent(this);
        }
    }

    public void RemoveParallelChild(TState stateId)
    {
        HSMState<TState, TTrigger> toRemove = GetImmediateChildState(stateId);

        if (parallelChilds.Remove(toRemove))
        {
            toRemove.RemoveParent(this);
        }
    }

    private void RemoveParent(HSMState<TState, TTrigger> parent)
    {
        parents.Remove(parent);
    }

    private bool DefaultComparer<T>(T arg1, T arg2) where T : unmanaged
    {
        return arg1.Equals(arg2);
    }

    public void SetStateComparer(Func<TState, TState, bool> comparerMethod)
    {
        if(comparerMethod == null)
        {
            throw new NullReferenceException("comparer method was null");
        }

        stateIdComparer = comparerMethod;
    }

    public void SetTriggerComparer(Func<TTrigger, TTrigger, bool> comparerMethod)
    {
        if (comparerMethod == null)
        {
            throw new NullReferenceException("comparer method was null");
        }

        triggerComparer = comparerMethod;
    }

    public HSMState<TState, TTrigger> GetNonParallelActiveLeaf()
    {
        HSMState<TState, TTrigger> current = this;
        
        while(current.ActiveNonParallelChild != null)
        {
            current = current.ActiveNonParallelChild;
        }

        return current;
    }

    public HSMState<TState, TTrigger> GetRoot()
    {
        HSMState<TState, TTrigger> current = this;

        while (current.parents.Count > 0)
        {
            current = current.parents[0];
        }

        return current;
    }

    public virtual HSMState<TState, TTrigger> GetImmediateChildState(TState state)
    {
        for(int i = 0; i < parallelChilds.Count; i++)
        {
            var current = parallelChilds[i];

            if(stateIdComparer(current.StateId, state))
            {
                return current;
            }
        }

        for (int i = 0; i < childs.Count; i++)
        {
            HSMState<TState, TTrigger> current = childs[i];

            if (stateIdComparer(current.StateId, state))
            {
                return current;
            }
        }

        return default;
    }

    public bool ContainsImmediateChildState(TState state)
    {
        return GetImmediateChildState(state) != null;
    }

    public bool IsOnState(TState state)
    {
        var root = GetRoot();

        return root.InternalIsOnState(state);
    }

    private bool InternalIsOnState(TState state)
    {
        for(int i = 0; i < parallelChilds.Count; i++)
        {
            if(parallelChilds[i].InternalIsOnState(state))
            {
                return true;
            }
        }

        var current = this;

        while(current != null)
        {
            if(stateIdComparer(current.StateId, state))
            {
                return true;
            }

            current = current.ActiveNonParallelChild;
        }

        return false;
    }

    public void SetInitialState(TState stateId)
    {
        if(ContainsImmediateChildState(stateId))
        {
            InitialChildState = stateId;
        }
        else
        {
            throw new InvalidOperationException("Initial state was not added to state machine");
        }
        
    }

    public bool MakeChildTransition(TState childStateFrom, TTrigger trigger, TState childStateTo)
    {
        if(ContainsImmediateChildState(childStateFrom) && ContainsImmediateChildState(childStateTo))
        {
            HSMTransition<TState, TTrigger> transition = new HSMTransition<TState, TTrigger>(childStateFrom, trigger, childStateTo);

            if (!transitions.Contains(transition, stateIdComparer, triggerComparer))
            {
                transitions.Add(transition);
                return true;
            }
        }

        return false;
    }

    public bool MakeChildTransition(HSMTransition<TState, TTrigger> transition)
    {
        if (ContainsImmediateChildState(transition.stateFrom) && ContainsImmediateChildState(transition.stateTo))
        {
            if (!transitions.Contains(transition, stateIdComparer, triggerComparer))
            {
                transitions.Add(transition);
                return true;
            }
        }

        return false;
    }

    public bool ContainsChildTransition(TState childStateFrom, TTrigger trigger)
    {
        return GetTransition(childStateFrom, trigger) != null;
    }

    public bool IsChildStateAbleToTransitionateTo(TState childStateFrom, TState childStateTo)
    {
        for(int i = 0; i < transitions.Count; i++)
        {
            HSMTransition<TState, TTrigger> transition = transitions[i];

            if(stateIdComparer(childStateFrom, transition.stateFrom) && stateIdComparer(childStateTo, transition.stateTo))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsAbleToTransitionateTo(TState stateTo)
    {
        for(int i = 0; i < parents.Count; i++)
        {
            HSMState<TState, TTrigger> parent = parents[i];

            if(parent.IsChildStateAbleToTransitionateTo(StateId, stateTo))
            {
                return true;
            }
        }

        return false;
    }

    public void BreakTransition(TState stateFrom, TTrigger trigger)
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            HSMTransition<TState, TTrigger> current = transitions[i];

            if (stateIdComparer(current.stateFrom, stateFrom) && triggerComparer(current.trigger, trigger))
            {
                transitions.RemoveAt(i);
                break;
            }
        }
    }

    public void BreakAllTransitionFrom(TState state)
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            HSMTransition<TState, TTrigger> current = transitions[i];

            if (stateIdComparer(current.stateFrom, state) || stateIdComparer(current.stateTo, state))
            {
                transitions.RemoveAt(i);
                i--;
            }
        }
    }

    public HSMTransition<TState, TTrigger> GetTransition(TState stateFrom, TTrigger trigger)
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            var current = transitions[i];

            if (stateIdComparer(current.stateFrom, stateFrom) && triggerComparer(current.trigger, trigger))
            {
                return current;
            }
        }

        return default;
    }

    public bool SendEvent(TTrigger trigger)
    {
        return InternalSendEvent(trigger);
    }

    private bool InternalSendEvent(TTrigger trigger)
    {
        HSMState<TState, TTrigger> current = GetNonParallelActiveLeaf();

        while (current != null)
        {
            TriggerResponse response = current.HandleEvent(trigger);

            switch (response)
            {
                case TriggerResponse.Accept:

                    if (current.childs.Count > 0)
                    {
                        HSMTransition<TState, TTrigger> transition = current.GetTransition(current.ActiveNonParallelChild.StateId, trigger);

                        if (transition != null && transition.IsValidTransition())
                        {
                            current.Transitionate(transition);

                            return true;
                        }
                    }

                    for (int i = 0; i < current.parallelChilds.Count; i++)
                    {
                        if (current.parallelChilds[i].SendEvent(trigger))
                        {
                            return true;
                        }
                    }

                    break;

                case TriggerResponse.Reject:
                    return false;
            }

            current = current.ActiveParent;
        }

        return false;
    }

    protected virtual TriggerResponse HandleEvent(TTrigger trigger)
    {
        return TriggerResponse.Accept;
    }

    private void Transitionate(in HSMTransition<TState, TTrigger> transition)
    {
        ExitActiveNonParallelChild();

        ActiveNonParallelChild = GetImmediateChildState(transition.stateTo);

        EnterNonParallelActiveChild();

        if (onStateChanged != null)
        {
            onStateChanged(transition.stateFrom, transition.stateTo, transition.trigger);
        }
    }
}

public class HSMTransition<TState, TTrigger> : IEquatable<HSMTransition<TState, TTrigger>>, IEnumerable<GuardCondition> where TState : unmanaged where TTrigger : unmanaged
{
    public readonly TState stateFrom;
    public readonly TTrigger trigger;
    public readonly TState stateTo;

    protected List<GuardCondition> guardConditions;

    public HSMTransition(TState stateFrom, TTrigger trigger, TState stateTo)
    {
        this.stateFrom = stateFrom;
        this.trigger = trigger;
        this.stateTo = stateTo;

        guardConditions = new List<GuardCondition>();
    }

    public HSMTransition(TState stateFrom, TTrigger trigger)
    {
        this.stateFrom = stateFrom;
        this.trigger = trigger;
        stateTo = default;
    }

    public void AddGuardCondition(GuardCondition guardCondition)
    {
        if(guardConditions.Contains(guardCondition) == false)
        {
            guardConditions.Add(guardCondition);
        }
    }

    public void RemoveGuardCondition(GuardCondition guardCondition)
    {
        guardConditions.Remove(guardCondition);
    }

    public bool IsValidTransition()
    {
        for (int i = 0; i < guardConditions.Count; i++)
        {
            if(guardConditions[i].IsValid() == false)
            {
                return false;
            }
        }

        return true;
    }

    public bool Equals(HSMTransition<TState, TTrigger> other)
    {
        return stateFrom.Equals(other.stateFrom) && trigger.Equals(other.trigger);
    }

    public bool Equals(HSMTransition<TState, TTrigger> other, Func<TState, TState, bool> stateComparer, Func<TTrigger, TTrigger, bool> triggerComparer)
    {
        return stateComparer(stateFrom, other.stateFrom) && triggerComparer(trigger, other.trigger);
    }

    public IEnumerator<GuardCondition> GetEnumerator()
    {
        return guardConditions.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}