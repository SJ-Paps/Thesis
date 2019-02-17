using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalRunningState : TribalHSMState
{
    private float velocityConstraintPercentage = 180;

    private bool shouldExit;

    private int velocityConstraintId;

    public TribalRunningState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Animator.SetTrigger(Tribal.RunAnimatorTrigger);

        velocityConstraintId = Owner.MaxVelocity.AddPercentageConstraint(velocityConstraintPercentage);

        shouldExit = false;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (shouldExit)
        {
            SendEvent(Character.Trigger.Trot);
        }

        shouldExit = true;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Animator.ResetTrigger(Tribal.TrotAnimatorTrigger);

        Owner.MaxVelocity.RemovePercentageConstraint(velocityConstraintId);
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        switch (trigger)
        {
            case Character.Trigger.Run:

                shouldExit = false;

                return true;

            default:

                return false;
        }
    }
}