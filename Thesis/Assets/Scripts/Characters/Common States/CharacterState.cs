using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState : State<Character.State, Character.Trigger>, ISerializationCallbackReceiver
{
    protected Character character;
    protected List<Character.Order> orders;
    protected Character.Blackboard blackboard;

    protected CharacterState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard) : base(fsm, state)
    {
        SetCharacterData(fsm, state, character, orders, blackboard);
    }

    protected CharacterState() : base(null, Character.State.Idle)
    {

    }

    public void SetCharacterData(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        this.character = character;
        this.orders = orders;
        this.blackboard = blackboard;

        stateMachine = fsm;
        InnerState = state;
    }

    protected override void OnEnter()
    {

    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnExit()
    {
        
    }

    public virtual void OnBeforeSerialize()
    {

    }

    public virtual void OnAfterDeserialize()
    {

    }
}
