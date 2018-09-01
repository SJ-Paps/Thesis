using SAM.FSM;
using UnityEngine;

public class FallingState : CharacterState
{
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D collider;
    private float groundDetectionDistance = 0.1f;
    private float diameter;

    public FallingState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state, character)
    {
        rigidbody2D = character.GetComponent<Rigidbody2D>();
        collider = character.GetComponent<BoxCollider2D>();
        diameter = collider.size.y / 2;
    }

    

    protected override void OnEnter(ref Character.ChangedStateEventArgs e)
    {
        EditorDebug.Log("FALLING ENTER");
    }

    protected override void OnExit()
    {

    }

    protected override void OnUpdate()
    {
        float xRay = character.transform.position.x;
        float yRay = character.transform.position.y - diameter;

        RaycastHit2D groundDetection = Physics2D.Raycast(new Vector2(xRay, yRay), Vector2.down, groundDetectionDistance);

        if (groundDetection.transform != null)
        {
            stateMachine.Trigger(Character.Trigger.Ground);
        }
    }
}
