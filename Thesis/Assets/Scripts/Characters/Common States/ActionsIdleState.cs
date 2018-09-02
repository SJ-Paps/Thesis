using SAM.FSM;
using UnityEngine;

public class ActionsIdleState : CharacterState 
{
    public ActionsIdleState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state, character)
    {

    }

    protected override void OnUpdate()
    {
        while (eventQueue.Count != 0) {
            Character.Order ev = eventQueue.Dequeue();

            if (ev == Character.Order.OrderHide) {
                stateMachine.Trigger(Character.Trigger.Hide);
            }
        }
    }
}
