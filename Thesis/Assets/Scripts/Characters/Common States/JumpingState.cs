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
    private RaycastHit2D raycastHit2D;
    private float circlecastRadius = 0.1f;

    private int ledgeLayer = 1 << Reg.ledgeLayer;

    public JumpingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
        collider = character.GetComponent<Collider2D>();
        height = collider.bounds.size.y;
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
        EditorDebug.DrawLine(character.CheckerForGrapple.position, (Vector2)character.CheckerForGrapple.position + (Vector2)character.CheckerForGrapple.right * circlecastRadius, Color.red);
        EditorDebug.DrawLine(character.CheckerForGrapple.position, (Vector2)character.CheckerForGrapple.position + ((Vector2)character.CheckerForGrapple.right * -1) * circlecastRadius, Color.red);
        EditorDebug.DrawLine(character.CheckerForGrapple.position, (Vector2)character.CheckerForGrapple.position + ((Vector2)character.CheckerForGrapple.up * -1) * circlecastRadius, Color.red);
        EditorDebug.DrawLine(character.CheckerForGrapple.position, (Vector2)character.CheckerForGrapple.position + (Vector2)character.CheckerForGrapple.up * circlecastRadius, Color.red);

        CheckingForLedge();
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
        CheckingForLedge();

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

    private void CheckingForLedge() 
    {
        raycastHit2D = Physics2D.CircleCast(character.CheckerForGrapple.position, circlecastRadius, character.CheckerForGrapple.right, 0, ledgeLayer);

        if(raycastHit2D != false)
        {
            EditorDebug.Log(raycastHit2D.collider.transform.gameObject);
            stateMachine.Trigger(Character.Trigger.Grapple);
        }
    }
}
