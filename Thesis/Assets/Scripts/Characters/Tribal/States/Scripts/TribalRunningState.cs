public class TribalRunningState : TribalHSMState
{
    private float velocityConstraintPercentage = 180;

    private int velocityConstraintId;

    protected override void OnEnter()
    {
        base.OnEnter();

        Configuration.Animator.SetTrigger(Tribal.RunAnimatorTrigger);

        velocityConstraintId = Owner.MaxVelocity.AddPercentageConstraint(velocityConstraintPercentage);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();

        Configuration.Animator.ResetTrigger(Tribal.TrotAnimatorTrigger);

        Owner.MaxVelocity.RemovePercentageConstraint(velocityConstraintId);
    }
}