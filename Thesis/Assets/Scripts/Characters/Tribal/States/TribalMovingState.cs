using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class TribalMovingState : CharacterState
{
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private float moveForce = 0.2f, xVelocityDeadZone = 0.1f;

    private Animator animator;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        rigidbody2D = character.RigidBody2D;

        animator = character.Animator;
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
        Vector2 moveForceVector = new Vector2(moveForce, 0);

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order order = orders[i];

            if (order == Character.Order.OrderMoveLeft)
            {
                character.Face(true);

                blackboard.movingHorizontal = true;

                rigidbody2D.AddForce(moveForceVector * -1, ForceMode2D.Impulse);
                break;
            }
            else if (order == Character.Order.OrderMoveRight)
            {
                character.Face(false);

                blackboard.movingHorizontal = true;

                rigidbody2D.AddForce(moveForceVector, ForceMode2D.Impulse);
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
            rigidbody2D.AddForce(-1 * moveForceVector, ForceMode2D.Impulse);
        }
        else if (rigidbody2D.velocity.x <= -character.MovementVelocity)
        {
            rigidbody2D.AddForce(moveForceVector, ForceMode2D.Impulse);
        }

        if (rigidbody2D.velocity.x > -xVelocityDeadZone && rigidbody2D.velocity.x < xVelocityDeadZone)
        {
            stateMachine.Trigger(Character.Trigger.StopMoving);
        }

        base.OnUpdate();
    }
}
