using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState : State<Character.State, Character.Trigger>
{
    protected Character character;
    protected List<Character.Order> orders;
    protected Character.Blackboard blackboard;
    protected Animator animator;

    protected CharacterState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard) : base(fsm, state)
    {
        this.character = character;
        this.orders = orders;
        this.blackboard = blackboard;

        animator = character.GetComponent<Animator>();
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
