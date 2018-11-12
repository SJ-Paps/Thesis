using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class JumpingState : CharacterState {

    private bool jumping;
    private Rigidbody2D rigidbody2D;
    private Collider2D collider;
    private float maxHeight;
    private float maxVelocity = 4;
    private float height;
    private Vector2 jumpForce = Vector2.up * 0.8f;

    private Animator animator;

    public JumpingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
        collider = character.GetComponent<Collider2D>();
        height = collider.bounds.size.y;

        animator = character.Animator;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        animator.SetTrigger("Jump");

        EditorDebug.Log("JUMPING ENTER");

        float initPosY = character.transform.position.y;

        maxHeight = initPosY + height / 2.2f;

        rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
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
                rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);

                if (character.transform.position.y < maxHeight)
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
