public class TurretMovingState : CharacterHSMState
{
    private Turret turret;

    public TurretMovingState(Character.State state, string debugName = null) : base(state, debugName)
    {

    }

    protected override void OnUpdate()
    {
        /*for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft)
            {
                float rotation = character.MovementVelocity * Time.deltaTime;

                turret.Rotate(rotation);
            }
            else if(order == Character.Order.OrderMoveRight)
            {
                float rotation = -character.MovementVelocity * Time.deltaTime;

                turret.Rotate(rotation);
            }
            else
            {
                stateMachine.Trigger(Character.Trigger.StopMoving);
            }
        }*/
    }
}
