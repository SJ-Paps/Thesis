public class TribalTrottingState : TribalHSMState
{
    

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Animator.SetTrigger(Tribal.TrotAnimatorTrigger);
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Animator.ResetTrigger(Tribal.TrotAnimatorTrigger);
    }
}
