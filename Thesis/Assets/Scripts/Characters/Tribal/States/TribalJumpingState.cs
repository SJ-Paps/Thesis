using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class TribalJumpingState : CharacterState {

    private bool jumping;
    private Rigidbody2D rigidbody2D;
    private Collider2D collider;

    [SerializeField]
    private float maxHeight = 1, maxVelocity = 4, jumpForce = 0.8f, circlecastRadius = 0.1f, yVelocityDeadZone = 0.1f;

    private int ledgeLayer;

    private float currentMaxHeight;

    private Animator animator;

    private Action onFixedUpdateDelegate;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        rigidbody2D = character.RigidBody2D;
        collider = character.Collider;

        animator = character.Animator;

        ledgeLayer = 1 << Reg.ledgeLayer;

        onFixedUpdateDelegate = ApplyForce;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        animator.SetTrigger("Jump");

        EditorDebug.Log("JUMPING ENTER");

        float initPosY = character.transform.position.y;

        currentMaxHeight = initPosY + maxHeight;

        Vector2 jumpForceVector = new Vector2(0, jumpForce);

        rigidbody2D.AddForce(jumpForceVector, ForceMode2D.Impulse);

        character.onFixedUpdate += onFixedUpdateDelegate;
    }

    protected override void OnUpdate()
    {
        CheckingForLedge();
        Jump();

        base.OnUpdate();
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Jump");

        character.onFixedUpdate -= onFixedUpdateDelegate;
    }

    private void Jump()
    {
        jumping = false;

        for(int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderJump)
            {
                if (character.transform.position.y < currentMaxHeight)
                {
                    jumping = true;
                    break;
                }
            }
        }

        if(rigidbody2D.velocity.y > maxVelocity)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxVelocity);
        }

        if (!jumping || rigidbody2D.velocity.y < yVelocityDeadZone)
        {
            stateMachine.Trigger(Character.Trigger.Fall);
        }
    }

    private void ApplyForce()
    {
        Vector2 jumpForceVector = new Vector2(0, jumpForce);

        rigidbody2D.AddForce(jumpForceVector, ForceMode2D.Impulse);
    }

    private void CheckingForLedge()
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
    }
}
