public class TribalWalkingState : TribalHSMState
{
    private float velocityConstraintPercentage = 60;

    private int velocityConstraintId;

    public TribalWalkingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Animator.SetTrigger(Tribal.WalkAnimatorTrigger);

        velocityConstraintId = Owner.MaxVelocity.AddPercentageConstraint(velocityConstraintPercentage);
        
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Animator.ResetTrigger(Tribal.WalkAnimatorTrigger);

        Owner.MaxVelocity.RemovePercentageConstraint(velocityConstraintId);
    }
}