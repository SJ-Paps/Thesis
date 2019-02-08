using SAM.FSM;
using System.Collections.Generic;

public abstract class CharacterHSMState : HSMState<Character.State, Character.Trigger>
{
    protected Character character;
    protected Character.Trigger LastEnteringTrigger { get; private set; }
    protected Character.Trigger LastTrigger { get; private set; }

    protected CharacterHSMState(Character.State state, string debugName = null) : base(state, debugName)
    {
    }

    public void PropagateCharacterReference(Character reference)
    {
        CharacterHSMState root = (CharacterHSMState)GetRoot();

        root.InternalPropagateCharacterReference(reference);
    }

    private void InternalPropagateCharacterReference(Character reference)
    {
        for(int i = 0; i < parallelChilds.Count; i++)
        {
            ((CharacterHSMState)parallelChilds[i]).InternalPropagateCharacterReference(reference);
        }

        for (int i = 0; i < childs.Count; i++)
        {
            ((CharacterHSMState)childs[i]).InternalPropagateCharacterReference(reference);
        }
        
        character = reference;

        InternalOnCharacterReferencePropagated();
    }

    private void InternalOnCharacterReferencePropagated()
    {
        onAnyStateChanged += UpdateLastTriggers;
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

    private void UpdateLastTriggers(Character.State stateFrom, Character.State stateTo, Character.Trigger trigger)
    {
        if(stateTo == StateId)
        {
            LastEnteringTrigger = trigger;
        }

        LastTrigger = trigger;
    }
}
