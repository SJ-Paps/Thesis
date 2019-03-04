using System;
using UnityEngine;

public class TribalJumpingState : TribalHSMState
{

    private bool isOrderingJump;

    private float currentMaxHeight;

    private Action onFixedUpdateDelegate;

    private float velocityDeadZone = 0.002f;

    public TribalJumpingState(byte stateId, string debugName) : base(stateId, debugName)
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        currentMaxHeight = Owner.transform.position.y + Owner.JumpMaxHeight;

        Owner.onFixedUpdate += onFixedUpdateDelegate;

        Owner.Animator.SetTrigger(Tribal.JumpAnimatorTrigger);
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

        Owner.Animator.ResetTrigger(Tribal.JumpAnimatorTrigger);
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
        return Owner.transform.position.y < currentMaxHeight && Owner.RigidBody2D.velocity.y > velocityDeadZone && isOrderingJump;
    }

    private void OnFixedUpdate()
    {
        Jump();

        isOrderingJump = false;
    }

    private void Jump()
    {
        Owner.RigidBody2D.AddForce(new Vector2(0, Owner.JumpAcceleration), ForceMode2D.Impulse);
    }

    /*private void CheckingForLedge()
    {
        Bounds bounds = character.Collider.bounds;
        float xDir = character.transform.right.x;

        Vector2 detectionPoint = new Vector2(bounds.center.x + ((bounds.extents.x + 0.1f) * xDir), bounds.center.y + bounds.extents.y);

        Collider2D auxColl = Physics2D.OverlapPoint(detectionPoint, ledgeLayer);

        if (auxColl != null)
        {
            //EditorDebug.Log("checkeando");
            blackboard.LastLedgeDetected = auxColl;
            stateMachine.Trigger(Character.Trigger.Grapple);
        }
    }*/
}
