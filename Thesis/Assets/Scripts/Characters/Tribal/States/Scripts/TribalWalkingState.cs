using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalWalkingState : TribalHSMState
{
    private float velocityConstraintPercentage = 60;

    private bool shouldExit;

    private int velocityConstraintId;

    public TribalWalkingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        character.Animator.SetTrigger(Tribal.WalkAnimatorTrigger);

        velocityConstraintId = character.AddVelocityConstraintByPercentageAndGetConstraintId(velocityConstraintPercentage);

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

        character.Animator.ResetTrigger(Tribal.WalkAnimatorTrigger);

        character.RemoveVelocityConstraintById(velocityConstraintId);
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        switch(trigger)
        {
            case Character.Trigger.Walk:

                shouldExit = false;

                return true;

            default:

                return false;
        }
    }
}