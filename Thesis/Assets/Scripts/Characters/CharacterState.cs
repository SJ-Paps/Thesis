using SAM.FSM;
using SAM;
using System;
using System.Collections.Generic;

public abstract class CharacterState : State<Character.State, Character.Trigger>
{
    protected Character character;
    protected List<Character.Order> orders;

    protected CharacterState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders) : base(fsm, state)
    {
        this.character = character;
        this.orders = orders;
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
}
