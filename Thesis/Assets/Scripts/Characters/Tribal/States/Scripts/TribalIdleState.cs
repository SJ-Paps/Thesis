using UnityEngine;

public class TribalIdleState : TribalHSMState
{
    private Animator animator;

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        animator = Configuration.Animator;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        animator.SetTrigger(Tribal.IdleAnimatorTrigger);
    }

    protected override void OnExit()
    {
        base.OnExit();

        animator.ResetTrigger(Tribal.IdleAnimatorTrigger);
    }
}
