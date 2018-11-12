public class TurretIdleState : CharacterState
{

    protected override void OnEnter()
    {

    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft || order == Character.Order.OrderMoveRight)
            {
                stateMachine.Trigger(Character.Trigger.Move);
            }
        }
    }

    protected override void OnExit()
    {

    }
}
