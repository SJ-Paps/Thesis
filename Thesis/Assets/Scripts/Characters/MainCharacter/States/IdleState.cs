using SAM.FSM;

public class IdleState : CharacterState
{
    public IdleState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character) : base(fsm, state, character)
    {

    }

    protected override void OnEnter()
    {
        EditorDebug.Log("IDLE ENTER");
    }

    public override void Update()
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
