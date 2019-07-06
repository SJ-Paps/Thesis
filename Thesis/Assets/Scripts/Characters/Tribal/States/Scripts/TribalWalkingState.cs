public class TribalWalkingState : TribalHSMState
{
    private float velocityConstraintPercentage = 60;

    private int velocityConstraintId;

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Animator.SetTrigger(Tribal.WalkAnimatorTrigger);

        velocityConstraintId = Owner.MovementVelocity.AddPercentageConstraint(velocityConstraintPercentage);
        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Animator.ResetTrigger(Tribal.WalkAnimatorTrigger);

        Owner.MovementVelocity.RemovePercentageConstraint(velocityConstraintId);
    }
}