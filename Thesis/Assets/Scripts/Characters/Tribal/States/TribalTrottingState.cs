using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;
using System;

public class TribalTrottingState : TribalHSMState
{
    /*private Rigidbody2D rigidbody2D;

    [SerializeField]
    private float moveForce = 0.2f, xVelocityDeadZone = 0.1f;

    private Character.Trigger enteringOrder;
    private int forceDirectionMultiplier;
    private bool isGoingLeft;

    private Animator animator;

    private Action onFixedUpdateDelegate;*/

    public TribalTrottingState(Character.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        character.Animator.SetTrigger(Tribal.TrotAnimatorTriggerName);
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.Animator.ResetTrigger(Tribal.TrotAnimatorTriggerName);
    }

    /*protected override void OnUpdate()
    {
        if(character.IsGrounded)
        {
            for (int i = 0; i < orders.Count && character.CanMove; i++)
            {
                Character.Order order = orders[i];

                if (order == enteringOrder)
                {
                    if (character.IsGrounded)
                    {
                        character.Face(isGoingLeft);

                        blackboard.movingHorizontal = true;
                    }

                    return;
                }
                else if (order == Character.Order.OrderStopMoving || (isGoingLeft && order == Character.Order.OrderMoveRight) || (!isGoingLeft && order == Character.Order.OrderMoveLeft))
                {
                    stateMachine.Trigger(Character.Trigger.StopMoving);
                    return;
                }

            }

            blackboard.movingHorizontal = false;

            if (rigidbody2D.velocity.x > -xVelocityDeadZone && rigidbody2D.velocity.x < xVelocityDeadZone)
            {
                stateMachine.Trigger(Character.Trigger.StopMoving);
            }
        }
    }

    private void ApplyForce()
    {
        if(blackboard.movingHorizontal)
        {
            Vector2 moveForceVector = new Vector2(moveForce, 0);

            rigidbody2D.AddForce(moveForceVector * forceDirectionMultiplier, ForceMode2D.Impulse);

            if (rigidbody2D.velocity.x >= character.MovementVelocity)
            {
                rigidbody2D.AddForce(-1 * moveForceVector, ForceMode2D.Impulse);
            }
            else if (rigidbody2D.velocity.x <= -character.MovementVelocity)
            {
                rigidbody2D.AddForce(moveForceVector, ForceMode2D.Impulse);
            }
        }
    }*/
}
