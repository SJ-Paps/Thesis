﻿public class TribalRunningState : TribalHSMState
{
    private float velocityConstraintPercentage = 180;

    private int velocityConstraintId;

    public TribalRunningState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Animator.SetTrigger(Tribal.RunAnimatorTrigger);

        velocityConstraintId = Owner.MaxVelocity.AddPercentageConstraint(velocityConstraintPercentage);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Animator.ResetTrigger(Tribal.TrotAnimatorTrigger);

        Owner.MaxVelocity.RemovePercentageConstraint(velocityConstraintId);
    }
}