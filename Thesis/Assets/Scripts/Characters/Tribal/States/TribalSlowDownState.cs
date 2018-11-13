using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SAM.FSM;

[Serializable]
public class TribalSlowDownState : CharacterState
{
    private Character.Order enteringOrder;

    [SerializeField]
    private float xVelocityDeadZone = 0.1f;

    [Range(1, 5)]
    [SerializeField]
    private float brakeForce = 1f;

    private float xDir;

    private Rigidbody2D rigidbody2D;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        rigidbody2D = character.RigidBody2D;
    }

    protected override void OnEnter()
    {
        EditorDebug.Log("SLOW DOWN ENTER");

        Debug.Log(rigidbody2D.velocity);
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x / brakeForce, rigidbody2D.velocity.y);
        Debug.Log(rigidbody2D.velocity);
    }

    protected override void OnUpdate()
    {
        if (character.IsGrounded)
        {

            if (rigidbody2D.velocity.x > -xVelocityDeadZone && rigidbody2D.velocity.x < xVelocityDeadZone)
            {
                stateMachine.Trigger(Character.Trigger.StopMoving);
            }
        }
    }

    protected override void OnExit()
    {
        EditorDebug.Log("SLOW DOWN EXIT");
    }
}
