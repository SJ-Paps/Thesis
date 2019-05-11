﻿using System;
using UnityEngine;

public class TribalClimbingRopeState : TribalHSMState
{
    private Action onFixedUpdateDelegate;
    private int verticalDirection;
    private bool shouldStop;

    private Rope currentRope;

    private float currentClimbUpForce;
    private float currentClimbDownForce;

    private float climbUpDifficultyModifier = 20;
    private float climbDownDifficultyModifier = 8;

    public TribalClimbingRopeState()
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        currentRope = (Rope)Blackboard.activable;

        Owner.onFixedUpdate += onFixedUpdateDelegate;
        shouldStop = false;

        currentClimbUpForce = (Owner.TribalConfigurationData.ClimbForce / (currentRope.GetClimbDifficulty() * climbUpDifficultyModifier));
        currentClimbDownForce = (Owner.TribalConfigurationData.ClimbForce / (currentRope.GetClimbDifficulty() * climbDownDifficultyModifier));
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
        verticalDirection = 0;
    }

    private void OnFixedUpdate()
    {
        Rigidbody2D possiblyNewSegment = currentRope.GetNearestSegment(Owner.RigidBody2D.position);

        if (possiblyNewSegment != Blackboard.ropeHandler.RelativeOther.connectedBody)
        {
            Blackboard.ropeHandler.RelativeOther.autoConfigureOffset = true;
            Blackboard.ropeHandler.Connect(Owner.RigidBody2D, possiblyNewSegment);
            Blackboard.ropeHandler.RelativeOther.autoConfigureOffset = false;
            Blackboard.ropeHandler.RelativeOther.linearOffset = new Vector2(0, Blackboard.ropeHandler.RelativeOther.linearOffset.y);
        }

        if(verticalDirection == 1 && currentRope.IsNearTop(Owner.RigidBody2D.position, 0.1f) == false)
        {
            Blackboard.ropeHandler.RelativeOther.linearOffset = new Vector2(Blackboard.ropeHandler.RelativeOther.linearOffset.x,
                                                        Blackboard.ropeHandler.RelativeOther.linearOffset.y - (verticalDirection * currentClimbUpForce));
        }
        else if(verticalDirection == -1 && currentRope.IsNearBottom(Owner.RigidBody2D.position, 0.1f) == false)
        {
            Blackboard.ropeHandler.RelativeOther.linearOffset = new Vector2(Blackboard.ropeHandler.RelativeOther.linearOffset.x,
                                                        Blackboard.ropeHandler.RelativeOther.linearOffset.y - (verticalDirection * currentClimbDownForce));
        }
        else
        {
            shouldStop = true;
        }

        verticalDirection = 0;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.ClimbUp)
        {
            verticalDirection = 1;
            return true;
        }
        else if(trigger == Character.Order.ClimbDown)
        {
            verticalDirection = -1;
            return true;
        }
        else
        {
            return false;
        }
    }
}
