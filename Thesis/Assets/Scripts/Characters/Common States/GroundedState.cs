using SAM.FSM;
using UnityEngine;

public class GroundedState : CharacterState
{
    public GroundedState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state, character)
    {

    }

    protected override void OnEnter(ref Character.ChangedStateEventArgs e)
    {
        base.OnEnter(ref e);

        EditorDebug.Log("GROUNDED ENTER");
    }

    protected override void OnUpdate()
    {
        while (eventQueue.Count != 0)
        {
            Character.Order ev = eventQueue.Dequeue();

            if (ev == Character.Order.OrderJump)
            {
                stateMachine.Trigger(Character.Trigger.Jump);
            }
        }
    }
}