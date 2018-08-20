using SAM.FSM;
using System.Collections.Generic;

public abstract class CharacterState : State<Character.State, Character.Trigger>
{
    protected Character character;
    protected Queue<Character.Order> eventQueue;

    protected CharacterState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character) : base(fsm, state)
    {
        this.character = character;
        eventQueue = new Queue<Character.Order>();
    }

    public virtual void ProcessOrder(Character.Order e)
    {
        eventQueue.Enqueue(e);
    }

    protected override void OnExit()
    {
        eventQueue.Clear();
    }
}
