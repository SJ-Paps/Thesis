using SAM.FSM;
using UnityEngine;

public class JumpingState : CharacterState {

    private bool jumping;
    private Rigidbody2D rigidbody2D;
    private Collider2D collider;
    private float maxHeight;
    private float height;
    private Vector2 jumpForce = Vector2.up * 0.5f;

    public JumpingState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state, character)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
        collider = character.GetComponent<Collider2D>();
        height = collider.bounds.size.y;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnEnter(ref Character.ChangedStateEventArgs e)
    {
        EditorDebug.Log("JUMPING ENTER");

        float initPosY = character.transform.position.y;

        maxHeight = initPosY + height;

        rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
    }

    protected override void OnUpdate()
    {
        Jump();
    }

    private void Jump()
    {
        jumping = false;

        EditorDebug.Log(eventQueue.Count);

        while(eventQueue.Count != 0)
        {
            Character.Order ev = eventQueue.Dequeue();

            EditorDebug.Log(ev);

            if (ev == Character.Order.OrderJump)
            {
                rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);

                if (character.transform.position.y < maxHeight)
                {
                    jumping = true;
                }
            }
        }

        if (!jumping)
        {
            stateMachine.Trigger(Character.Trigger.Fall);
        }
    }

    
}
