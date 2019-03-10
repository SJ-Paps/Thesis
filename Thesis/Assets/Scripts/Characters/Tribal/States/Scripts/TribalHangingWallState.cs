using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalHangingWallState : TribalHSMState
{
    private float previousGravityScale;

    private Ladder currentLadder;

    protected override void OnEnter()
    {
        base.OnEnter();

        currentLadder = (Ladder)Blackboard.activable;

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
            Owner.RigidBody2D.AddForce(new Vector2(Owner.FacingDirection * Owner.TribalConfigurationData.JumpForceFromLadder, 0), ForceMode2D.Impulse);
        }

        return false;
    }

    private void ContraintPosition(Ladder ladder)
    {
        Vector2 offsetWorldPosition = (Vector2)Owner.transform.position + Owner.Collider.offset;

        if (ladder.Collider.OverlapPoint(offsetWorldPosition) == false)
        {
            ColliderDistance2D colliderDistance2D = Owner.Collider.Distance(ladder.Collider);

            Owner.RigidBody2D.MovePosition(colliderDistance2D.pointB + (Owner.Collider.offset * Owner.FacingDirection));
        }
    }
}
