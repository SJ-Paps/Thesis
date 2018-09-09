using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class FallingState : CharacterState
{
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D collider;
    private float groundDetectionDistance = 0.1f;
    private float diameter;

    public FallingState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList) : base(fsm, state, character, orderList)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
        collider = character.GetComponent<BoxCollider2D>();
        diameter = collider.bounds.size.y / 2;
    }

    protected override void OnEnter()
    {
        EditorDebug.Log("FALLING ENTER");
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnUpdate()
    {
        float xRay = character.transform.position.x;
        float yRay = character.transform.position.y - diameter;

        RaycastHit2D groundDetection = Physics2D.Raycast(new Vector2(xRay, yRay), Vector2.down , groundDetectionDistance, ~(1 << Reg.playerLayer));

        Debug.DrawRay(new Vector2(xRay, yRay), Vector2.down * groundDetectionDistance, Color.green);

        if (groundDetection.transform != null)
        {
            stateMachine.Trigger(Character.Trigger.Ground);
        }
    }
}
