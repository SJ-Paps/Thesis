using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class MovingState : CharacterState
{
    private RaycastHit2D raycastHit2D;
    private Rigidbody2D rigidbody2D;
    private Vector2 moveForce = new Vector2(0.2f, 0);

    private float xVelocityDeadZone = 0.1f;

    public MovingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        animator.SetTrigger("Move");

        EditorDebug.Log("MOVING ENTER");
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Move");

        blackboard.movingHorizontal = false;
    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft)
            {
                character.Face(true);

                blackboard.movingHorizontal = true;

                rigidbody2D.AddForce(moveForce * -1, ForceMode2D.Impulse);
                break;
            }
            else if (order == Character.Order.OrderMoveRight)
            {
                character.Face(false);

                blackboard.movingHorizontal = true;

                rigidbody2D.AddForce(moveForce, ForceMode2D.Impulse);
                break;
            }
            else
            {
                blackboard.movingHorizontal = false;
            }
        }

        if(orders.Count == 0)
        {
            blackboard.movingHorizontal = false;
        }

        if (rigidbody2D.velocity.x >= character.MovementVelocity)
        {
            rigidbody2D.AddForce(-1 * moveForce, ForceMode2D.Impulse);
        }
        else if (rigidbody2D.velocity.x <= -character.MovementVelocity)
        {
            rigidbody2D.AddForce(moveForce, ForceMode2D.Impulse);
        }

        if (rigidbody2D.velocity.x > -xVelocityDeadZone && rigidbody2D.velocity.x < xVelocityDeadZone)
        {
            stateMachine.Trigger(Character.Trigger.StopMoving);
        }

        base.OnUpdate();
    }
}
