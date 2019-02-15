using System;
using UnityEngine;

public class TribalJumpingState : TribalHSMState
{

    private bool isOrderingJump;

    private float currentMaxHeight;

    private Action onFixedUpdateDelegate;

    public TribalJumpingState(Character.State state, string debugName) : base(state, debugName)
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        currentMaxHeight = character.transform.position.y + character.JumpMaxHeight;

        character.onFixedUpdate += onFixedUpdateDelegate;

        character.Animator.SetTrigger(Tribal.JumpAnimatorTrigger);
    }

    protected override void OnUpdate()
    {
        if(ShouldContinueJumping() == false)
        {
            SendEvent(Character.Trigger.Fall);
        }

        base.OnUpdate();
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.onFixedUpdate -= onFixedUpdateDelegate;

        character.Animator.ResetTrigger(Tribal.JumpAnimatorTrigger);
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        switch(trigger)
        {
            case Character.Trigger.Jump:

                isOrderingJump = true;

                return TriggerResponse.Reject;

            default:

                return TriggerResponse.Accept;


        }
    }

    private bool ShouldContinueJumping()
    {
        return character.transform.position.y < currentMaxHeight && isOrderingJump;
    }

    private void OnFixedUpdate()
    {
        Jump();

        isOrderingJump = false;
    }

    private void Jump()
    {
        character.RigidBody2D.AddForce(new Vector2(0, character.JumpAcceleration), ForceMode2D.Impulse);
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
