using SAM.FSM;
using SAM.Timers;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingState : CharacterState 
{
    FSM<Character.State, Character.Trigger> characterMovementFSM;
    FSM<Character.State, Character.Trigger> characterActionFSM;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D collider2D;
    private SyncTimer timerOfReleasingLedge;
    private float cooldownForChangingToFallingState = 0.25f;
    private float verticalClimbingSpeed = 1.2f;
    private float horizontalClimbingSpeed = 1.2f;

    private int ledgeLayer = 1 << Reg.ledgeLayer;

    public GrapplingState(
        FSM<Character.State, Character.Trigger> fsm, 
        Character.State state, 
        Character character, 
        List<Character.Order> orderList, 
        FSM<Character.State, Character.Trigger> movementFSM, 
        FSM<Character.State, Character.Trigger> actionFSM,
        Character.Blackboard blackboard) : base(fsm, state, character, orderList, blackboard)
    {
        characterMovementFSM = movementFSM;
        characterActionFSM = actionFSM;
        rigidbody2D = character.GetComponent<Rigidbody2D>();
        collider2D = character.GetComponent<BoxCollider2D>();

        timerOfReleasingLedge = new SyncTimer();
        timerOfReleasingLedge.onTick += EnteringToFallingState;
        timerOfReleasingLedge.Interval = cooldownForChangingToFallingState;
    }

    protected override void OnEnter() 
    {
        EditorDebug.Log("GRAPPLING ENTER");
        characterMovementFSM.Active = false;
        characterActionFSM.Active = false;
        blackboard.isGrappled = true;
    }

    protected override void OnUpdate() {
        timerOfReleasingLedge.Update(Time.deltaTime);

        if(character.IsGrappled)
        {
            Grappled();
        }
        else if(character.IsClimbingLedge)
        {
            ClimbLedge();
        }
        else if(character.ReleasingLedge)
        {
            ReleaseLedge();
        }

        if(!character.ReleasingLedge && !character.IsClimbingLedge)
        {
            for(int i = 0; i < orders.Count; i++)
            {
                Character.Order order = orders[i];

                if(order == Character.Order.OrderGrapple)
                {
                    blackboard.isClimbingLedge = true;
                    blackboard.isGrappled = false;
                }
                else if(order == Character.Order.OrderReleaseLedge)
                {
                    blackboard.releasingLedge = true;
                    blackboard.isGrappled = false;
                    timerOfReleasingLedge.Start();
                }
            }
        }
    }

    protected override void OnExit() 
    {
        blackboard.isGrappled = false;
        blackboard.isClimbingLedge = false;
        blackboard.releasingLedge = false;
        characterMovementFSM.Active = true;
        characterActionFSM.Active = true;
        base.OnExit();
    }

    private void Grappled()
    {
        rigidbody2D.gravityScale = 0;
        rigidbody2D.velocity = Vector2.zero;
    }

    private void ReleaseLedge()
    {
        rigidbody2D.gravityScale = 1f;
        rigidbody2D.isKinematic = false;
    }

    private void EnteringToFallingState(SyncTimer timer)
    {
        EditorDebug.Log("ENTERING TO FALLING STATE");
        stateMachine.Trigger(Character.Trigger.Fall);
    }

    private void ClimbLedge()
    {

        if((collider2D.bounds.center.y - collider2D.bounds.extents.y) < (character.LastLedgeDetected.bounds.center.y + character.LastLedgeDetected.bounds.extents.y))
        {
            rigidbody2D.isKinematic = true;
            character.transform.Translate(new Vector3(0f, verticalClimbingSpeed * Time.deltaTime, 0f));
        }
        else if((collider2D.bounds.center.y + collider2D.bounds.extents.y) >= (character.LastLedgeDetected.bounds.center.y + character.LastLedgeDetected.bounds.extents.y))
        {
            if(!character.FacingLeft)
            {
                if((collider2D.bounds.center.x - collider2D.bounds.extents.x) < (character.LastLedgeDetected.bounds.center.x - character.LastLedgeDetected.bounds.extents.x))
                {
                    character.transform.Translate(new Vector3(horizontalClimbingSpeed * Time.deltaTime, 0f, 0f));
                }
                else
                {
                    rigidbody2D.gravityScale = 1f;
                    rigidbody2D.isKinematic = false;
                    stateMachine.Trigger(Character.Trigger.Fall);
                }
            }
            else
            {
                if((collider2D.bounds.center.x + collider2D.bounds.extents.x) > (character.LastLedgeDetected.bounds.center.x + character.LastLedgeDetected.bounds.extents.x))
                {
                    character.transform.Translate(new Vector3(horizontalClimbingSpeed * Time.deltaTime, 0f, 0f));
                }
                else
                {
                    rigidbody2D.gravityScale = 1f;
                    rigidbody2D.isKinematic = false;
                    stateMachine.Trigger(Character.Trigger.Fall);
                }
            }
        }
    }
}
