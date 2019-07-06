﻿public class TribalRunningState : TribalHSMState
{
    private float velocityConstraintPercentage = 180;

    private int velocityConstraintId;

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Animator.SetTrigger(Tribal.RunAnimatorTrigger);

        velocityConstraintId = Owner.MovementVelocity.AddPercentageConstraint(velocityConstraintPercentage);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Animator.ResetTrigger(Tribal.TrotAnimatorTrigger);

        Owner.MovementVelocity.RemovePercentageConstraint(velocityConstraintId);
    }
}