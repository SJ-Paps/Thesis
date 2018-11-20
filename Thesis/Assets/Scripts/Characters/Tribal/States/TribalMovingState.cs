﻿using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class TribalMovingState : CharacterState
{
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private float moveForce = 0.2f, xVelocityDeadZone = 0.1f;

    private Character.Order enteringOrder;
    private int forceDirectionMultiplier;
    private bool isGoingLeft;

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

        foreach (Character.Order order in orders)
        {
            if (order == Character.Order.OrderMoveLeft)
            {
                enteringOrder = order;
                forceDirectionMultiplier = -1;
                isGoingLeft = true;

                break;
            }
            else if(order == Character.Order.OrderMoveRight)
            {
                enteringOrder = order;
                forceDirectionMultiplier = 1;
                isGoingLeft = false;

                break;
            }
        }

        animator.SetTrigger("Move");

        EditorDebug.Log("MOVING ENTER");
    }

    protected override void OnExit()
    {
        EditorDebug.Log("MOVING EXIT");

        animator.ResetTrigger("Move");

        blackboard.movingHorizontal = false;
    }

    protected override void OnUpdate()
    {
        if(character.IsGrounded)
        {
            Vector2 moveForceVector = new Vector2(moveForce, 0);

            for (int i = 0; i < orders.Count; i++)
            {
                Character.Order order = orders[i];

                if (order == enteringOrder)
                {
                    if (character.IsGrounded)
                    {
                        character.Face(isGoingLeft);

                        blackboard.movingHorizontal = true;

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
}