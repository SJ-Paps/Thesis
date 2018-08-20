using SAM.FSM;
using UnityEngine;

public class GroundedState : CharacterState
{
    public GroundedState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character) : base(fsm, state, character)
    {

    }

    protected override void OnEnter()
    {
        EditorDebug.Log("GROUNDED ENTER");
    }

    public override void Update()
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
