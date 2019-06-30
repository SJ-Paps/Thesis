public class TribalTrottingState : TribalHSMState
{
    

    protected override void OnEnter()
    {
        base.OnEnter();

        Configuration.Animator.SetTrigger(Tribal.TrotAnimatorTrigger);
    }

    protected override void OnExit()
    {
        base.OnExit();

        Configuration.Animator.ResetTrigger(Tribal.TrotAnimatorTrigger);
    }
}
