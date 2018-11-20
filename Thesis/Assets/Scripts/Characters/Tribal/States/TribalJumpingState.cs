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
    private float maxHeight = 1, maxVelocity = 4, jumpForce = 0.8f, circlecastRadius = 0.1f;

    private int ledgeLayer;

    private float currentMaxHeight;

    private Animator animator;

    private Action<Collision2D> onCollisionDelegate;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        rigidbody2D = character.RigidBody2D;
        collider = character.Collider;

        animator = character.Animator;

        ledgeLayer = 1 << Reg.ledgeLayer;

        onCollisionDelegate = OnCollision;
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

        character.onCollisionStay2D += onCollisionDelegate;
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

        character.onCollisionStay2D -= onCollisionDelegate;
    }

    private void Jump()
    {
        jumping = false;

        for(int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderJump)
            {
                Vector2 jumpForceVector = new Vector2(0, jumpForce);

                rigidbody2D.AddForce(jumpForceVector, ForceMode2D.Impulse);

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

        if (!jumping)
        {
            stateMachine.Trigger(Character.Trigger.Fall);
        }
    }

    private void OnCollision(Collision2D collision)
    {
        for(int i = 0; i < collision.contacts.Length; i++)
        {
            float limit = 0.05f;

            float yCeiling = collider.bounds.extents.y;
            float xCeiling = collider.bounds.extents.x;

            Vector2 point = collision.contacts[i].point;

            float pointX = point.x;
            float pointY = point.y;

            float charY = character.transform.position.y + yCeiling - limit;
            float charX = character.transform.position.x + xCeiling - limit;

            if (pointY >= charY &&
                pointX >= charX)
            {
                stateMachine.Trigger(Character.Trigger.Fall);
            }
        }
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
