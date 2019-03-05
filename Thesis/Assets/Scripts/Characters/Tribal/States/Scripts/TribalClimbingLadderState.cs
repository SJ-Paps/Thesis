﻿using System;
using UnityEngine;

public class TribalClimbingLadderState : TribalHSMState
{
    private Character.Order verticalDirectionOrder;
    private Character.Order horizontalDirectionOrder;

    private Action onFixedUpdateDelegate;

    private Ladder currentLadder;

    private bool shouldStop;

    private float previousDrag;

    public TribalClimbingLadderState(byte stateId, string debugName = null) : base(stateId, debugName)
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        currentLadder = (Ladder)Blackboard.activable;

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

        horizontalDirectionOrder = default;
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

        if (verticalDirectionOrder == Character.Order.ClimbUp && CanMoveOnDirection(verticalDirectionOrder))
        {
            if (ownerRigidbody.velocity.y < 0)
            {
                ownerRigidbody.velocity = new Vector2(ownerRigidbody.velocity.x, 0);
            }

            ownerRigidbody.AddForce(new Vector2(0, Owner.TribalConfigurationData.ClimbForce / climbDifficulty));

            shouldStop = false;
        }
        else if (verticalDirectionOrder == Character.Order.ClimbDown && CanMoveOnDirection(verticalDirectionOrder))
        {
            if (ownerRigidbody.velocity.y > 0)
            {
                ownerRigidbody.velocity = new Vector2(ownerRigidbody.velocity.x, 0);
            }

            ownerRigidbody.AddForce(new Vector2(0, -1 * Owner.TribalConfigurationData.ClimbForce));

            shouldStop = false;
        }

        if (horizontalDirectionOrder == Character.Order.MoveLeft && CanMoveOnDirection(horizontalDirectionOrder))
        {
            if (ownerRigidbody.velocity.x > 0)
            {
                ownerRigidbody.velocity = new Vector2(0, ownerRigidbody.velocity.y);
            }

            ownerRigidbody.AddForce(new Vector2(-1 * Owner.TribalConfigurationData.ClimbForce / climbDifficulty, 0));
            Owner.Face(true);

            shouldStop = false;
        }
        else if (horizontalDirectionOrder == Character.Order.MoveRight && CanMoveOnDirection(horizontalDirectionOrder))
        {
            if (ownerRigidbody.velocity.x < 0)
            {
                ownerRigidbody.velocity = new Vector2(0, ownerRigidbody.velocity.y);
            }

            ownerRigidbody.AddForce(new Vector2(Owner.TribalConfigurationData.ClimbForce / climbDifficulty, 0));
            Owner.Face(false);

            shouldStop = false;
        }

    }

    private void ClampVelocity()
    {
        Vector2 velocity = Owner.RigidBody2D.velocity;
        float maxClimbVelocityPositive = Owner.TribalConfigurationData.ClimbForce / 2;
        float maxClimbVelocityNegative = Owner.TribalConfigurationData.ClimbForce / 2 * -1;

        if (velocity.x > maxClimbVelocityPositive)
        {
            Debug.Log("AA");
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
        else if(trigger == Character.Order.MoveRight || trigger == Character.Order.MoveLeft)
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
        else if (orderDirection == Character.Order.MoveLeft)
        {
            if (currentLadder.Collider.OverlapPoint(new Vector2(offsetWorldPosition.x - snap, offsetWorldPosition.y)))
            {
                return true;
            }
        }
        else if (orderDirection == Character.Order.MoveRight)
        {
            if (currentLadder.Collider.OverlapPoint(new Vector2(offsetWorldPosition.x + snap, offsetWorldPosition.y)))
            {
                return true;
            }
        }

        return false;
    }
}
