using UnityEngine;

public class TribalHangingLadderState : TribalHSMState
{
    private float previousGravityScale;

    private Ladder currentLadder;

    protected override void OnEnter()
    {
        base.OnEnter();

        currentLadder = (Ladder)Blackboard.GetItemOf<IActivable>("Activable");
        
        Owner.RigidBody2D.velocity = new Vector2(0, 0);

        previousGravityScale = Owner.RigidBody2D.gravityScale;
        Owner.RigidBody2D.gravityScale = 0;

        ContraintPosition(currentLadder);
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.RigidBody2D.gravityScale = previousGravityScale;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if (trigger == Character.Order.Jump)
        {
            Owner.RigidBody2D.AddForce(new Vector2(Owner.FacingDirection * Owner.JumpForceFromLadder, 0), ForceMode2D.Impulse);
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
        Owner.RigidBody2D.MovePosition(new Vector2(((Vector2)ladder.transform.position + ladder.Collider.offset).x, Owner.transform.position.y));
    }
}
