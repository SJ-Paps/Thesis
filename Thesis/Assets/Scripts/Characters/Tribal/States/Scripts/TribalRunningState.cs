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

        character.Animator.SetTrigger(Tribal.RunAnimatorTriggerName);

        velocityConstraintId = character.AddVelocityConstraintByPercentageAndGetConstraintId(velocityConstraintPercentage);

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

        character.Animator.ResetTrigger(Tribal.TrotAnimatorTriggerName);

        character.RemoveVelocityConstraintById(velocityConstraintId);
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        switch (trigger)
        {
            case Character.Trigger.Run:

                shouldExit = false;

                return TriggerResponse.Reject;

            default:

                return TriggerResponse.Accept;
        }
    }
}