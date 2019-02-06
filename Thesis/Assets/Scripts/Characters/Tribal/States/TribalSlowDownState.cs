using System;
using UnityEngine;

[Serializable]
public class TribalSlowDownState : TribalHSMState
{
    private Character.Trigger enteringOrder;

    [SerializeField]
    private float xVelocityDeadZone = 0.1f;

    [Range(1, 5)]
    [SerializeField]
    private float brakeForce = 1f;

    private float xDir;

    private Rigidbody2D rigidbody2D;

    public TribalSlowDownState(Character.State state, string debugName) : base(state, debugName)
    {
        rigidbody2D = character.RigidBody2D;
    }

    /*protected override void OnEnter()
    {
        EditorDebug.Log("SLOW DOWN ENTER");
        
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x / brakeForce, rigidbody2D.velocity.y);
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
    }*/
}
