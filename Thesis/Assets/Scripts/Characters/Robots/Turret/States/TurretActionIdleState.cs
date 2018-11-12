public class TurretActionIdleState : CharacterState
{

    protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if(order == Character.Order.OrderAttack)
            {
                stateMachine.Trigger(Character.Trigger.Attack);
            }
        }
    }
}
