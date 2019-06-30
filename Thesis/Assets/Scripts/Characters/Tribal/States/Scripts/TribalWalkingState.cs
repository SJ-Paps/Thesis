public class TribalWalkingState : TribalHSMState
{
    private float velocityConstraintPercentage = 60;

    private int velocityConstraintId;

    protected override void OnEnter()
    {
        base.OnEnter();

        Configuration.Animator.SetTrigger(Tribal.WalkAnimatorTrigger);

        velocityConstraintId = Owner.MaxVelocity.AddPercentageConstraint(velocityConstraintPercentage);
        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();

        Configuration.Animator.ResetTrigger(Tribal.WalkAnimatorTrigger);

        Owner.MaxVelocity.RemovePercentageConstraint(velocityConstraintId);
    }
}