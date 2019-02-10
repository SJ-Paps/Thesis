public abstract class CharacterHSMState : HSMState<Character.State, Character.Trigger>, IOwnable<Character>
{
    protected Character character;
    protected Character.Trigger LastEnteringTrigger { get; private set; }
    protected bool activeDebug;

    public Character Owner
    {
        get
        {
            return character;
        }
    }

    protected Character.Blackboard blackboard;

    protected CharacterHSMState(Character.State state, string debugName = null) : base(state, debugName)
    {
    }

    public void PropagateOwnerReference(Character reference)
    {
        CharacterHSMState root = (CharacterHSMState)GetRoot();

        root.InternalPropagateOwnerReference(reference);
    }

    private void InternalPropagateOwnerReference(Character reference)
    {
        for(int i = 0; i < parallelChilds.Count; i++)
        {
            ((CharacterHSMState)parallelChilds[i]).InternalPropagateOwnerReference(reference);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((CharacterHSMState)childs[i]).InternalPropagateOwnerReference(reference);
        }

        for (int i = 0; i < transitions.Count; i++)
        {
            foreach(CharacterGuardCondition guardCondition in transitions[i])
            {
                guardCondition.PropagateOwnerReference(reference);
            }
        }
        
        character = reference;

        InternalOnCharacterReferencePropagated();
    }

    private void InternalOnCharacterReferencePropagated()
    {
        onAnyStateChanged += CatchEnteringTrigger;
        OnOwnerReferencePropagated();
    }

    protected virtual void OnOwnerReferencePropagated()
    {
        
    }

    protected override void OnEnter()
    {
#if UNITY_EDITOR
        if(activeDebug)
        {
            EditorDebug.Log(DebugName + " ENTER " + character.name);
        }
#endif
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
#if UNITY_EDITOR
        if(activeDebug)
        {
            EditorDebug.Log(DebugName + " EXIT " + character.name);
        }
#endif
    }

    private void CatchEnteringTrigger(Character.State stateFrom, Character.State stateTo, Character.Trigger trigger)
    {
        if(stateTo == StateId)
        {
            LastEnteringTrigger = trigger;

            for(int i = 0; i < parallelChilds.Count; i++)
            {
                ((CharacterHSMState)parallelChilds[i]).LastEnteringTrigger = trigger;
            }

            if(ActiveNonParallelChild != null)
            {
                ((CharacterHSMState)ActiveNonParallelChild).LastEnteringTrigger = trigger;
            }
        }
    }

    public void PropagateBlackboardReference(Character.Blackboard blackboard)
    {
        CharacterHSMState root = (CharacterHSMState)GetRoot();

        root.InternalPropagateBlackboardReference(blackboard);
    }

    private void InternalPropagateBlackboardReference(Character.Blackboard blackboard)
    {
        for (int i = 0; i < parallelChilds.Count; i++)
        {
            ((CharacterHSMState)parallelChilds[i]).InternalPropagateBlackboardReference(blackboard);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((CharacterHSMState)childs[i]).InternalPropagateBlackboardReference(blackboard);
        }

        for (int i = 0; i < transitions.Count; i++)
        {
            foreach (CharacterGuardCondition guardCondition in transitions[i])
            {
                guardCondition.PropagateBlackboardReference(blackboard);
            }
        }

        this.blackboard = blackboard;
    }
}
