using SAM.FSM;
using SAM.Timers;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TribalGrapplingState : CharacterState 
{
    FSM<Character.State, Character.Trigger> characterMovementFSM;
    FSM<Character.State, Character.Trigger> characterActionFSM;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D collider2D;
    private SyncTimer timerOfReleasingLedge;

    [SerializeField]
    private float cooldownForChangingToFallingState = 0.25f, verticalClimbingSpeed = 1.2f, horizontalClimbingSpeed = 1.2f;

    private int ledgeLayer;

    public void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state,Character character,List<Character.Order> orderList,FSM<Character.State, Character.Trigger> movementFSM,FSM<Character.State, Character.Trigger> actionFSM, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orderList, blackboard);

        characterMovementFSM = movementFSM;
        characterActionFSM = actionFSM;

        rigidbody2D = character.RigidBody2D;
        collider2D = (BoxCollider2D)character.Collider;

        ledgeLayer = 1 << Reg.ledgeLayer;

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

        Grappled();
    }

    protected override void OnUpdate() {
        timerOfReleasingLedge.Update(Time.deltaTime);

        if(character.IsClimbingLedge)
        {
            ClimbLedge();
        }

        if(!character.IsClimbingLedge && character.IsGrappled)
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
                    ReleaseLedge();
                    timerOfReleasingLedge.Start();
                }
            }
        }
    }

    protected override void OnExit() 
    {
        EditorDebug.Log("GRAPLING EXIT");

        timerOfReleasingLedge.Stop();

        base.OnExit();
    }

    private void Grappled()
    {
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.gravityScale = 0;
    }

    private void ReleaseLedge()
    {
        blackboard.isClimbingLedge = false;
        blackboard.isGrappled = false;
        rigidbody2D.gravityScale = 1f;
        rigidbody2D.isKinematic = false;

        characterMovementFSM.Active = true;
        characterActionFSM.Active = true;
    }

    private void EnteringToFallingState(SyncTimer timer)
    {
        EditorDebug.Log("ENTERING TO FALLING STATE");
        stateMachine.Trigger(Character.Trigger.Fall);
    }

    private void ClimbLedge()
    {
        Collider2D LastLedgeDetected = blackboard.LastLedgeDetected;

        if((collider2D.bounds.center.y - collider2D.bounds.extents.y) < (LastLedgeDetected.bounds.center.y + LastLedgeDetected.bounds.extents.y))
        {
            rigidbody2D.isKinematic = true;
            character.transform.Translate(new Vector3(0f, verticalClimbingSpeed * Time.deltaTime, 0f));
        }
        else if((collider2D.bounds.center.y + collider2D.bounds.extents.y) >= (LastLedgeDetected.bounds.center.y + LastLedgeDetected.bounds.extents.y))
        {
            if(!character.FacingLeft)
            {
                if((collider2D.bounds.center.x - collider2D.bounds.extents.x) < (LastLedgeDetected.bounds.center.x - LastLedgeDetected.bounds.extents.x))
                {
                    character.transform.Translate(new Vector3(horizontalClimbingSpeed * Time.deltaTime, 0f, 0f));
                }
                else
                {
                    ReleaseLedge();
                    stateMachine.Trigger(Character.Trigger.Fall);
                }
            }
            else
            {
                if((collider2D.bounds.center.x + collider2D.bounds.extents.x) > (LastLedgeDetected.bounds.center.x + LastLedgeDetected.bounds.extents.x))
                {
                    character.transform.Translate(new Vector3(horizontalClimbingSpeed * Time.deltaTime, 0f, 0f));
                }
                else
                {
                    ReleaseLedge();
                    stateMachine.Trigger(Character.Trigger.Fall);
                }
            }
        }
    }
}
