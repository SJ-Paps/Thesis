using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class MovingState : CharacterState
{
    private Rigidbody2D rigidbody2D;
    private Vector2 moveForce = new Vector2(0.2f, 0);
    private float maxVelocityX = 3f;

    public MovingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList) : base(fsm, state, character, orderList)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        //EditorDebug.Log("MOVING ENTER");
    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft)
            {
                rigidbody2D.AddForce(moveForce * -1, ForceMode2D.Impulse);
                break;
            }
            else if (order == Character.Order.OrderMoveRight)
            {
                rigidbody2D.AddForce(moveForce, ForceMode2D.Impulse);
                break;
            }
        }

        if (rigidbody2D.velocity.x >= maxVelocityX)
        {
            rigidbody2D.AddForce(moveForce * -1, ForceMode2D.Impulse);
        }
        else if (rigidbody2D.velocity.x <= -maxVelocityX)
        {
            rigidbody2D.AddForce(moveForce, ForceMode2D.Impulse);
        }

        if (rigidbody2D.velocity.x == 0)
        {
            stateMachine.Trigger(Character.Trigger.StopMoving);
        }

        base.OnUpdate();
    }
}
