using SAM.FSM;
using UnityEngine;

public class MovingState : CharacterState
{
    private Rigidbody2D rigidbody2D;
    private Vector2 moveForce = new Vector2(0.2f, 0);
    private float maxVelocityX = 3f;

    public MovingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character) : base(fsm, state, character)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
    }

    protected override void OnEnter()
    {
        EditorDebug.Log("MOVING ENTER");
    }

    public override void Update()
    {
        while (eventQueue.Count != 0)
        {
            Character.Order ev = eventQueue.Dequeue();

            if (ev == Character.Order.OrderMoveLeft)
            {
                rigidbody2D.AddForce(moveForce * -1, ForceMode2D.Impulse);
            }
            else if (ev == Character.Order.OrderMoveRight)
            {
                rigidbody2D.AddForce(moveForce, ForceMode2D.Impulse);
            }
        }

        if(rigidbody2D.velocity.x >= maxVelocityX)
        {
            rigidbody2D.AddForce(moveForce * -1, ForceMode2D.Impulse);
        }
        else if(rigidbody2D.velocity.x <= -maxVelocityX)
        {
            rigidbody2D.AddForce(moveForce, ForceMode2D.Impulse);
        }

        if(rigidbody2D.velocity.x == 0)
        {
            stateMachine.Trigger(Character.Trigger.StopMoving);
        }
    }
}
