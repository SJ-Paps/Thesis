using SAM.FSM;
using SAM.Timers;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ActionsIdleState : CharacterState 
{
    private FSM<Character.State, Character.Trigger> characterJumpingFSM;
    private FSM<Character.State, Character.Trigger> characterMovementFSM;
    private Action<Collider2D> checkingForEnteringToTheHidingPlaceMethod;
    private Action<Collider2D> checkingForExitingOfTheHidingPlaceMethod;
    private Rigidbody2D characterRigidBody2D;
    private SyncTimer timerOfHiding;
    private RaycastHit2D raycastHit2D;
    private float raycastDistance;
    private float cooldownOfHiding;
    private bool canHide;

    public ActionsIdleState(FSM<Character.State, Character.Trigger> fsm,
        Character.State state,
        Character character,
        List<Character.Order> orders,
        FSM<Character.State,Character.Trigger> jumpingFSM,
        FSM<Character.State, Character.Trigger> movementFSM,
        Character.Blackboard blackboard) : base(fsm, state, character, orders, blackboard)
    {
        characterJumpingFSM = jumpingFSM;
        characterMovementFSM = movementFSM;
        checkingForEnteringToTheHidingPlaceMethod += CheckingForEnteringToTheHidingPlace;
        checkingForExitingOfTheHidingPlaceMethod += CheckingForExitingOfTheHidingPlace;
        characterRigidBody2D = character.GetComponent<Rigidbody2D>();
        timerOfHiding = new SyncTimer();
        raycastDistance = 0.4f;
        cooldownOfHiding = 2.0f;
        canHide = false;
        raycastHit2D = new RaycastHit2D();
        timerOfHiding.Interval = cooldownOfHiding;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        character.onTriggerEnter2D += checkingForEnteringToTheHidingPlaceMethod;
        character.onTriggerExit2D += checkingForExitingOfTheHidingPlaceMethod;
        timerOfHiding.onTick += EnteringToTheHidingPlace;
        Physics2D.queriesStartInColliders = false;
        EditorDebug.Log("ACTIONIDLE ENTER");
    }

    protected override void OnExit() 
    {
        base.OnExit();
        character.onTriggerEnter2D -= checkingForEnteringToTheHidingPlaceMethod;
        character.onTriggerExit2D -= checkingForExitingOfTheHidingPlaceMethod;
        timerOfHiding.onTick -= EnteringToTheHidingPlace;
       // EditorDebug.Log("ACTIONIDLE EXIT");
    }

    protected override void OnUpdate()
    {

        timerOfHiding.Update(Time.deltaTime);

        raycastHit2D = Physics2D.Raycast(character.transform.position, (Vector2)character.transform.right, raycastDistance);
        EditorDebug.DrawLine(character.transform.position, (Vector2)character.transform.localPosition + (Vector2)character.transform.right * raycastDistance, Color.red);

        if(raycastHit2D && raycastHit2D.collider.gameObject.layer == Reg.objectLayer && character.IsGrounded == true)
        {
            stateMachine.Trigger(Character.Trigger.Push);
            blackboard.isPushing = true;
        }

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderAction && canHide == true && character.IsGrounded == true && !timerOfHiding.Active) 
            {
                EditorDebug.Log("LLAMADO AL TIMER ACTION");
                characterMovementFSM.Active = false;
                characterJumpingFSM.Active = false;
                characterRigidBody2D.velocity = new Vector2(0, characterRigidBody2D.velocity.y);
                timerOfHiding.Start();
            }
        }
    }

    void CheckingForEnteringToTheHidingPlace(Collider2D collider2D) 
    {
        if (collider2D.gameObject.layer == Reg.hidingPlaceLayer) 
        {
            canHide = true;
        }
    }

    void CheckingForExitingOfTheHidingPlace(Collider2D collider2D)
    {
        if(collider2D.gameObject.layer == Reg.hidingPlaceLayer)
        {
            canHide = false;
        }
    }
    
    void EnteringToTheHidingPlace(SyncTimer timer) 
    {
        stateMachine.Trigger(Character.Trigger.Hide);
    }
}
