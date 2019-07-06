using System;
using UnityEngine;

public class TribalClimbingLadderState : TribalHSMState
{
    private Character.Order verticalDirectionOrder;

    private Action onFixedUpdateDelegate;

    private Ladder currentLadder;

    private bool shouldStop;

    private float previousDrag;

    public TribalClimbingLadderState()
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        currentLadder = (Ladder)Blackboard.GetItemOf<IActivable>("Activable");

        shouldStop = false;

        Owner.onFixedUpdate += onFixedUpdateDelegate;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(shouldStop)
        {
            SendEvent(Character.Order.FinishAction);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.onFixedUpdate -= onFixedUpdateDelegate;
    }

    private void OnFixedUpdate()
    {
        ExecuteOrders();

        ClampVelocity();
        
        verticalDirectionOrder = default;
    }

    private void ExecuteOrders()
    {
        float climbDifficulty = currentLadder.GetClimbDifficulty();

        if (climbDifficulty == 0)
        {
            climbDifficulty = 1;
        }

        Rigidbody2D ownerRigidbody = Owner.RigidBody2D;

        shouldStop = true;

        if(CanMoveOnDirection(verticalDirectionOrder))
        {
            if (verticalDirectionOrder == Character.Order.ClimbUp)
            {
                if (ownerRigidbody.velocity.y < 0)
                {
                    ClampVelocityAxis(true);
                }

                ownerRigidbody.AddForce(new Vector2(0, Owner.ClimbForce / climbDifficulty));
            }
            else if (verticalDirectionOrder == Character.Order.ClimbDown)
            {
                if (ownerRigidbody.velocity.y > 0)
                {
                    ClampVelocityAxis(true);
                }

                ownerRigidbody.AddForce(new Vector2(0, -1 * Owner.ClimbForce));
            }

            shouldStop = false;
        }
        else
        {
            ClampVelocityAxis(true);
        }

    }

    private void ClampVelocity()
    {
        Vector2 velocity = Owner.RigidBody2D.velocity;
        float maxClimbVelocityPositive = Owner.ClimbForce / 2;
        float maxClimbVelocityNegative = Owner.ClimbForce / 2 * -1;

        if (velocity.y > maxClimbVelocityPositive)
        {
            velocity = new Vector2(velocity.x, maxClimbVelocityPositive);
        }
        else if (velocity.y < maxClimbVelocityNegative)
        {
            velocity = new Vector2(velocity.x, maxClimbVelocityNegative);
        }

        Owner.RigidBody2D.velocity = velocity;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        bool isDirectionOrder = false;

        if (trigger == Character.Order.ClimbUp || trigger == Character.Order.ClimbDown)
        {
            verticalDirectionOrder = trigger;
            isDirectionOrder = true;
        }

        return isDirectionOrder;
    }

    private bool CanMoveOnDirection(Character.Order orderDirection)
    {
        Vector2 offsetWorldPosition = (Vector2)Owner.transform.position + Owner.Collider.offset;
        float snap = 0.1f;

        if (orderDirection == Character.Order.ClimbUp)
        {
            if (currentLadder.Collider.OverlapPoint(new Vector2(offsetWorldPosition.x, offsetWorldPosition.y + snap)))
            {
                return true;
            }
        }
        else if (orderDirection == Character.Order.ClimbDown)
        {
            if (currentLadder.Collider.OverlapPoint(new Vector2(offsetWorldPosition.x, offsetWorldPosition.y - snap)))
            {
                return true;
            }
        }

        return false;
    }

    private void ClampVelocityAxis(bool vertical)
    {
        if(vertical)
        {
            Owner.RigidBody2D.velocity = new Vector2(Owner.RigidBody2D.velocity.x, 0);
        }
        else
        {
            Owner.RigidBody2D.velocity = new Vector2(0, Owner.RigidBody2D.velocity.y);
        }
    }
}
