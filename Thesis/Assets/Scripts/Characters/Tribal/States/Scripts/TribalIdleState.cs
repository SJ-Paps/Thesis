using UnityEngine;

public class TribalIdleState : TribalHSMState
{
    private Animator animator;

    public TribalIdleState(byte stateId, string debugName) : base(stateId, debugName)
    {
        
    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        animator = Owner.Animator;
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
