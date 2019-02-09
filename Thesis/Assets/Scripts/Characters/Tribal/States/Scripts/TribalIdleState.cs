using UnityEngine;

public class TribalIdleState : TribalHSMState
{
    private Animator animator;

    public TribalIdleState(Character.State state, string debugName) : base(state, debugName)
    {
        
    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        animator = character.Animator;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        animator.SetTrigger(Tribal.IdleAnimatorTriggerName);
    }

    protected override void OnExit()
    {
        base.OnExit();

        animator.ResetTrigger(Tribal.IdleAnimatorTriggerName);
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        return base.HandleEvent(trigger);
    }
}
