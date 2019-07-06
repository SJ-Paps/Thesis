using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalHangingWallState : TribalHSMState
{
    private float previousGravityScale;

    private ClimbableWall currentWall;

    protected override void OnEnter()
    {
        base.OnEnter();

        currentWall = (ClimbableWall)Blackboard.GetItemOf<IActivable>("Activable");

        Owner.RigidBody2D.velocity = new Vector2(0, 0);

        previousGravityScale = Owner.RigidBody2D.gravityScale;
        Owner.RigidBody2D.gravityScale = 0;

        ContraintPosition(currentWall);
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

        return false;
    }

    private void ContraintPosition(ClimbableWall wall)
    {
        Vector2 offsetWorldPosition = (Vector2)Owner.transform.position + Owner.Collider.offset;

        if (wall.Collider.OverlapPoint(offsetWorldPosition) == false)
        {
            ColliderDistance2D colliderDistance2D = Owner.Collider.Distance(wall.Collider);

            Owner.RigidBody2D.MovePosition(colliderDistance2D.pointB + (Owner.Collider.offset * Owner.FacingDirection));
        }
    }
}
