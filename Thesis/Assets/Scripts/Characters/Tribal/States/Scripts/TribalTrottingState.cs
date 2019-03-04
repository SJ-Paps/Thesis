public class TribalTrottingState : TribalHSMState
{

    public TribalTrottingState(byte stateId, string debugName) : base(stateId, debugName)
    {

    }

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
