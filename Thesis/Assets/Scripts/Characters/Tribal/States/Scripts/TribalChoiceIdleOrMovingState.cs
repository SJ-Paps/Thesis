public class TribalChoiceIdleOrMovingState : TribalHSMState
{

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        if(Owner.RigidBody2D.velocity.x > 0)
        {
            SendEvent(Character.Order.MoveRight);
        }
        else if(Owner.RigidBody2D.velocity.x < 0)
        {
            SendEvent(Character.Order.MoveLeft);
        }
        else
        {
            SendEvent(Character.Order.StopMoving);
        }
    }
}