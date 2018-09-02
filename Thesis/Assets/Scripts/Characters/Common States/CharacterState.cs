using SAM.FSM;
using System.Collections.Generic;
using System;

public abstract class CharacterState : State<Character.State, Character.Trigger, Character.ChangedStateEventArgs>
{
    protected Character character;
    protected Queue<Character.Order> eventQueue;

    protected Action<Character.Order> processOrderDelegate;

    protected CharacterState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state)
    {
        this.character = character;
        eventQueue = new Queue<Character.Order>();

        processOrderDelegate = ProcessOrder;
    }

    protected override void OnEnter(ref Character.ChangedStateEventArgs e)
    {
        character.onProcessOrder += processOrderDelegate;
    }

    protected virtual void ProcessOrder(Character.Order e)
    {
        eventQueue.Enqueue(e);
    }

    protected override void OnExit()
    {
        eventQueue.Clear();
        character.onProcessOrder -= processOrderDelegate;
    }
}
