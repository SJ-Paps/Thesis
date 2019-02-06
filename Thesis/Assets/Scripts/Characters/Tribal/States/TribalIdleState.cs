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
        animator.SetTrigger("Idle");

        EditorDebug.Log("IDLE ENTER " + character.name);
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Idle");

        EditorDebug.Log("IDLE EXIT " + character.name);
    }
}
