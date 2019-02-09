using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalWalkingState : TribalHSMState
{
    private float velocityConstraintPercentage = 90;

    private bool shouldExit;

    public TribalWalkingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        character.Animator.SetTrigger(Tribal.TrotAnimatorTriggerName);

        character.AddVelocityConstraintByPercentage(velocityConstraintPercentage * -1);

        shouldExit = false;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(shouldExit)
        {
            SendEvent(Character.Trigger.Trot);
        }

        shouldExit = true;
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.Animator.ResetTrigger(Tribal.TrotAnimatorTriggerName);

        character.AddVelocityConstraintByPercentage(velocityConstraintPercentage);
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        switch(trigger)
        {
            case Character.Trigger.Walk:

                shouldExit = false;

                return TriggerResponse.Reject;

            default:

                return TriggerResponse.Accept;
        }
    }
}