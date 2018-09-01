using SAM.FSM;

public class CharacterIdleState : CharacterState
{
    public CharacterIdleState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state, character)
    {

    }

    protected override void OnEnter(ref Character.ChangedStateEventArgs e)
    {
        base.OnEnter(ref e);

        EditorDebug.Log("IDLE ENTER");
    }

    protected override void OnUpdate()
    {
        while (eventQueue.Count != 0)
        {
            Character.Order ev = eventQueue.Dequeue();

            if (ev == Character.Order.OrderMoveLeft || ev == Character.Order.OrderMoveRight)
            {
                stateMachine.Trigger(Character.Trigger.Move);
            }
        }
    }
}
