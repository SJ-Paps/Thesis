using SAM.FSM;
using System.Collections.Generic;

public abstract class CharacterState : State<Character.State, Character.Trigger>
{
    protected Character character;
    protected List<Character.Order> orders;
    protected Character.Blackboard blackboard;

    protected CharacterState() : base(null, Character.State.Idle)
    {

    }

    public virtual void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
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
}
