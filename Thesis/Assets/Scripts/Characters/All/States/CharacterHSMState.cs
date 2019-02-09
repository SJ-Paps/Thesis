public abstract class CharacterHSMState : HSMState<Character.State, Character.Trigger>
{
    protected Character character;
    protected Character.Trigger LastEnteringTrigger { get; private set; }

    protected Character.Blackboard blackboard;

    protected CharacterHSMState(Character.State state, string debugName = null) : base(state, debugName)
    {
    }

    public void PropagateCharacterReference(Character reference, Character.Blackboard blackboard)
    {
        CharacterHSMState root = (CharacterHSMState)GetRoot();

        root.InternalPropagateCharacterReference(reference, blackboard);
    }

    private void InternalPropagateCharacterReference(Character reference, Character.Blackboard blackboard)
    {
        for(int i = 0; i < parallelChilds.Count; i++)
        {
            ((CharacterHSMState)parallelChilds[i]).InternalPropagateCharacterReference(reference, blackboard);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((CharacterHSMState)childs[i]).InternalPropagateCharacterReference(reference, blackboard);
        }
        
        character = reference;
        this.blackboard = blackboard;

        InternalOnCharacterReferencePropagated();
    }

    private void InternalOnCharacterReferencePropagated()
    {
        onAnyStateChanged += CatchEnteringTrigger;
        OnCharacterReferencePropagated();
    }

    protected virtual void OnCharacterReferencePropagated()
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
}
