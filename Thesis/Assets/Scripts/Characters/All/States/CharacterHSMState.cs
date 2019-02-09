public abstract class CharacterHSMState : HSMState<Character.State, Character.Trigger>, IOwnable<Character>
{
    protected Character character;
    protected Character.Trigger LastEnteringTrigger { get; private set; }

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

    public void PropagateOwnerReference(Character reference, Character.Blackboard blackboard)
    {
        CharacterHSMState root = (CharacterHSMState)GetRoot();

        root.InternalPropagateOwnerReference(reference, blackboard);
    }

    private void InternalPropagateOwnerReference(Character reference, Character.Blackboard blackboard)
    {
        for(int i = 0; i < parallelChilds.Count; i++)
        {
            ((CharacterHSMState)parallelChilds[i]).InternalPropagateOwnerReference(reference, blackboard);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((CharacterHSMState)childs[i]).InternalPropagateOwnerReference(reference, blackboard);
        }

        for (int i = 0; i < transitions.Count; i++)
        {
            foreach(CharacterGuardCondition guardCondition in transitions[i])
            {
                guardCondition.PropagateOwnerReference(character);
            }
        }
        
        character = reference;
        this.blackboard = blackboard;

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
        EditorDebug.Log(DebugName + " ENTER " + character.name);
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnExit()
    {
        EditorDebug.Log(DebugName + " EXIT " + character.name);
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

    public void PropagateOwnerReference(Character ownerReference)
    {
        throw new System.NotImplementedException();
    }
}
