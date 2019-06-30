using System;
using UnityEngine;

public class TribalJumpingState : TribalHSMState
{

    private bool isOrderingJump;

    private float currentMaxHeight;

    private Action onFixedUpdateDelegate;

    private float velocityDeadZone = 0.002f;

    public TribalJumpingState()
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        currentMaxHeight = Owner.transform.position.y + Configuration.JumpMaxHeight;

        Owner.onFixedUpdate += onFixedUpdateDelegate;

        Configuration.Animator.SetTrigger(Tribal.JumpAnimatorTrigger);
    }

    protected override void OnUpdate()
    {
        if(ShouldContinueJumping() == false)
        {
            SendEvent(Character.Order.Fall);
        }

        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.onFixedUpdate -= onFixedUpdateDelegate;

        Configuration.Animator.ResetTrigger(Tribal.JumpAnimatorTrigger);
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Jump)
        {
            isOrderingJump = true;
            return true;
        }

        return false;
    }

    private bool ShouldContinueJumping()
    {
        return Owner.transform.position.y < currentMaxHeight && Configuration.RigidBody2D.velocity.y > velocityDeadZone && isOrderingJump;
    }

    private void OnFixedUpdate()
    {
        Jump();

        isOrderingJump = false;
    }

    private void Jump()
    {
        Configuration.RigidBody2D.AddForce(new Vector2(0, Configuration.JumpAcceleration), ForceMode2D.Impulse);
    }
}
