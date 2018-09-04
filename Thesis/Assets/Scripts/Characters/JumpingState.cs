using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class JumpingState : CharacterState {

    private bool jumping;
    private Rigidbody2D rigidbody2D;
    private Collider2D collider;
    private float maxHeight;
    private float height;
    private Vector2 jumpForce = Vector2.up * 0.5f;

    public JumpingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList) : base(fsm, state, character, orderList)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
        collider = character.GetComponent<Collider2D>();
        height = collider.bounds.size.y;
    }
    

    protected override void OnEnter()
    {
        base.OnEnter();

        EditorDebug.Log("JUMPING ENTER");

        float initPosY = character.transform.position.y;

        maxHeight = initPosY + height / 3;

        rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
    }

    protected override void OnUpdate()
    {
        Jump();

        base.OnUpdate();
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

        if (!jumping)
        {
            stateMachine.Trigger(Character.Trigger.Fall);
        }
    }

    
}
