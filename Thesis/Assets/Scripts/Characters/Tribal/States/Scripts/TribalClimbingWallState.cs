using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TribalClimbingWallState : TribalHSMState
{
    private Character.Order verticalDirectionOrder;
    private Character.Order horizontalDirectionOrder;

    private Action onFixedUpdateDelegate;

    private ClimbableWall currentWall;

    private bool shouldStop;

    private float previousDrag;

    public TribalClimbingWallState()
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        currentWall = (ClimbableWall)Blackboard.activable;

        shouldStop = false;

        Owner.onFixedUpdate += onFixedUpdateDelegate;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (shouldStop)
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

        horizontalDirectionOrder = default;
        verticalDirectionOrder = default;
    }

    private void ExecuteOrders()
    {
        float climbDifficulty = currentWall.GetClimbDifficulty();

        if (climbDifficulty == 0)
        {
            climbDifficulty = 1;
        }

        Rigidbody2D ownerRigidbody = Owner.RigidBody2D;

        shouldStop = true;

        if (CanMoveOnDirection(verticalDirectionOrder))
        {
            if (verticalDirectionOrder == Character.Order.ClimbUp)
            {
                if (ownerRigidbody.velocity.y < 0)
                {
                    ClampVelocityAxis(true);
                }

                ownerRigidbody.AddForce(new Vector2(0, Owner.TribalConfigurationData.ClimbForce / climbDifficulty));
            }
            else if (verticalDirectionOrder == Character.Order.ClimbDown)
            {
                if (ownerRigidbody.velocity.y > 0)
                {
                    ClampVelocityAxis(true);
                }

                ownerRigidbody.AddForce(new Vector2(0, -1 * Owner.TribalConfigurationData.ClimbForce));
            }

            shouldStop = false;
        }
        else
        {
            ClampVelocityAxis(true);
        }

        if (CanMoveOnDirection(horizontalDirectionOrder))
        {
            if (horizontalDirectionOrder == Character.Order.MoveLeft)
            {
                if (ownerRigidbody.velocity.x > 0)
                {
                    ClampVelocityAxis(false);
                }

                ownerRigidbody.AddForce(new Vector2(-1 * Owner.TribalConfigurationData.ClimbForce / climbDifficulty, 0));
                Owner.Face(true);
            }
            else if (horizontalDirectionOrder == Character.Order.MoveRight)
            {
                if (ownerRigidbody.velocity.x < 0)
                {
                    ClampVelocityAxis(false);
                }

                ownerRigidbody.AddForce(new Vector2(Owner.TribalConfigurationData.ClimbForce / climbDifficulty, 0));
                Owner.Face(false);
            }

            shouldStop = false;
        }
        else
        {
            ClampVelocityAxis(false);
        }

    }

    private void ClampVelocity()
    {
        Vector2 velocity = Owner.RigidBody2D.velocity;
        float maxClimbVelocityPositive = Owner.TribalConfigurationData.ClimbForce / 2;
        float maxClimbVelocityNegative = Owner.TribalConfigurationData.ClimbForce / 2 * -1;

        if (velocity.x > maxClimbVelocityPositive)
        {
            velocity = new Vector2(maxClimbVelocityPositive, velocity.y);
        }
        else if (velocity.x < maxClimbVelocityNegative)
        {
            velocity = new Vector2(maxClimbVelocityNegative, velocity.y);
        }

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
        else if (trigger == Character.Order.MoveRight || trigger == Character.Order.MoveLeft)
        {
            horizontalDirectionOrder = trigger;
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
            if (currentWall.Collider.OverlapPoint(new Vector2(offsetWorldPosition.x, offsetWorldPosition.y + snap)))
            {
                return true;
            }
        }
        else if (orderDirection == Character.Order.ClimbDown)
        {
            if (currentWall.Collider.OverlapPoint(new Vector2(offsetWorldPosition.x, offsetWorldPosition.y - snap)))
            {
                return true;
            }
        }
        else if (orderDirection == Character.Order.MoveLeft)
        {
            if (currentWall.Collider.OverlapPoint(new Vector2(offsetWorldPosition.x - snap, offsetWorldPosition.y)))
            {
                return true;
            }
        }
        else if (orderDirection == Character.Order.MoveRight)
        {
            if (currentWall.Collider.OverlapPoint(new Vector2(offsetWorldPosition.x + snap, offsetWorldPosition.y)))
            {
                return true;
            }
        }

        return false;
    }

    private void ClampVelocityAxis(bool vertical)
    {
        if (vertical)
        {
            Owner.RigidBody2D.velocity = new Vector2(Owner.RigidBody2D.velocity.x, 0);
        }
        else
        {
            Owner.RigidBody2D.velocity = new Vector2(0, Owner.RigidBody2D.velocity.y);
        }
    }
}
