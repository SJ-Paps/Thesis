using SAM.FSM;
using System.Collections.Generic;

public class CharacterIdleState : CharacterState
{
    public CharacterIdleState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList) : base(fsm, state, character, orderList)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        EditorDebug.Log("IDLE ENTER");
    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft || order == Character.Order.OrderMoveRight)
            {
                stateMachine.Trigger(Character.Trigger.Move);
                break;
            }
        }

        base.OnUpdate();
    }
}
