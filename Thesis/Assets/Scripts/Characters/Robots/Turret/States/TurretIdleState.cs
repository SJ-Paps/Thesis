using System;

public class TurretIdleState : CharacterHSMState
{

    public TurretIdleState(Character.State state, string debugName) : base(state, debugName)
    {

    }

    /*protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft || order == Character.Order.OrderMoveRight)
            {
                stateMachine.Trigger(Character.Trigger.Move);
            }
        }
    }*/
}
