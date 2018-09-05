using SAM.FSM;
using System.Collections.Generic;

public class GroundedState : CharacterState
{
    public GroundedState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList) : base(fsm, state, character, orderList)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        EditorDebug.Log("GROUNDED ENTER");
    }

    protected override void OnUpdate()
    {
        if(character.CheckIsOnFloor() == false)
        {
            stateMachine.Trigger(Character.Trigger.Fall);
        }

        for(int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderJump)
            {
                stateMachine.Trigger(Character.Trigger.Jump);
                break;
            }
        }
    }
}
