using UnityEngine;

public class TribalHangingLadderState : TribalHSMState
{
    private float previousGravityScale;

    private Ladder currentLadder;

    protected override void OnEnter()
    {
        base.OnEnter();

        currentLadder = (Ladder)Blackboard.GetItemOf<IActivable>("Activable");

        Configuration.RigidBody2D.velocity = new Vector2(0, 0);

        previousGravityScale = Configuration.RigidBody2D.gravityScale;
        Configuration.RigidBody2D.gravityScale = 0;

        ContraintPosition(currentLadder);
    }

    protected override void OnExit()
    {
        base.OnExit();

        Configuration.RigidBody2D.gravityScale = previousGravityScale;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if (trigger == Character.Order.Jump)
        {
            Configuration.RigidBody2D.AddForce(new Vector2(Owner.FacingDirection * Configuration.JumpForceFromLadder, 0), ForceMode2D.Impulse);
        }
        else if(trigger == Character.Order.MoveLeft)
        {
            Owner.Face(true);
        }
        else if(trigger == Character.Order.MoveRight)
        {
            Owner.Face(false);
        }

        return false;
    }

    private void ContraintPosition(Ladder ladder)
    {
        Configuration.RigidBody2D.MovePosition(new Vector2(((Vector2)ladder.transform.position + ladder.Collider.offset).x, Owner.transform.position.y));
    }
}
