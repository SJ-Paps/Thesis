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
    private float maxHeight = 1, maxVelocity = 4, jumpForce = 0.8f;

    private float currentMaxHeight;

    private Animator animator;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        rigidbody2D = character.RigidBody2D;
        collider = character.Collider;

        animator = character.Animator;
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
    }

    protected override void OnUpdate()
    {
        Jump();

        base.OnUpdate();
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Jump");
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
}
