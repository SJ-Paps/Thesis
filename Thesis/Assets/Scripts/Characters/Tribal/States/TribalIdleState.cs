using UnityEngine;

public class TribalIdleState : TribalHSMState
{
    private Animator animator;

    public TribalIdleState(Character.State state, string debugName) : base(state, debugName)
    {
        
    }

    protected override void OnCharacterReferencePropagated()
    {
        base.OnCharacterReferencePropagated();

        animator = character.Animator;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        animator.SetTrigger("Idle");
    }

    protected override void OnExit()
    {
        base.OnExit();

        animator.ResetTrigger("Idle");
    }
}
