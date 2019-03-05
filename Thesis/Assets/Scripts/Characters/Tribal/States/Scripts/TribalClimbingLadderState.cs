using System;
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

        shouldStop = true;
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

        if(verticalDirectionOrder == Character.Order.ClimbUp)
        {
            if(ownerRigidbody.velocity.y < 0)
            {
                ownerRigidbody.velocity = new Vector2(ownerRigidbody.velocity.x, 0);
            }

            ownerRigidbody.AddForce(new Vector2(0, Owner.TribalConfigurationData.ClimbForce / climbDifficulty));
        }
        else if(verticalDirectionOrder == Character.Order.ClimbDown)
        {
            if(ownerRigidbody.velocity.y > 0)
            {
                ownerRigidbody.velocity = new Vector2(ownerRigidbody.velocity.x, 0);
            }
            
            ownerRigidbody.AddForce(new Vector2(0, -1 * Owner.TribalConfigurationData.ClimbForce));
        }

        if(horizontalDirectionOrder == Character.Order.MoveLeft)
        {
            if(ownerRigidbody.velocity.x > 0)
            {
                ownerRigidbody.velocity = new Vector2(0, ownerRigidbody.velocity.y);
            }

            ownerRigidbody.AddForce(new Vector2(-1 * Owner.TribalConfigurationData.ClimbForce / climbDifficulty, 0));
            Owner.Face(true);
        }
        else if(horizontalDirectionOrder == Character.Order.MoveRight)
        {
            if(ownerRigidbody.velocity.x < 0)
            {
                ownerRigidbody.velocity = new Vector2(0, ownerRigidbody.velocity.y);
            }

            ownerRigidbody.AddForce(new Vector2(Owner.TribalConfigurationData.ClimbForce / climbDifficulty, 0));
            Owner.Face(false);
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
            shouldStop = false;
            isDirectionOrder = true;
        }
        else if(trigger == Character.Order.MoveRight || trigger == Character.Order.MoveLeft)
        {
            horizontalDirectionOrder = trigger;
            shouldStop = false;
            isDirectionOrder = true;
        }

        return isDirectionOrder;
    }
}
