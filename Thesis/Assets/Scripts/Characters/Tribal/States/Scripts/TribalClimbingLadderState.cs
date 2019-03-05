using System;
using UnityEngine;

public class TribalClimbingLadderState : TribalHSMState
{
    private Character.Order directionOrder;
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
        float climbDifficulty = currentLadder.GetClimbDifficulty();

        if(climbDifficulty == 0)
        {
            climbDifficulty = 1;
        }

        switch(directionOrder)
        {
            case Character.Order.ClimbUp:

                Owner.RigidBody2D.AddForce(new Vector2(0, 1 / climbDifficulty));
                break;

            case Character.Order.ClimbDown:

                Owner.RigidBody2D.AddForce(new Vector2(0, -1));
                break;

            case Character.Order.MoveRight:

                Owner.RigidBody2D.AddForce(new Vector2(1 / climbDifficulty, 0));
                break;

            case Character.Order.MoveLeft:

                Owner.RigidBody2D.AddForce(new Vector2(-1 / climbDifficulty, 0));
                break;
        }

        shouldStop = true;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        bool isDirectionOrder = trigger == Character.Order.ClimbUp || trigger == Character.Order.ClimbDown
            || trigger == Character.Order.MoveLeft || trigger == Character.Order.MoveRight;

        directionOrder = trigger;

        shouldStop = !isDirectionOrder;

        return isDirectionOrder;
    }
}
