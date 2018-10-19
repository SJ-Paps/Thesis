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
    private Action onCharacterDetectedMethod;
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
        onCharacterDetectedMethod = OnCharacterDetected;

        characterRigidBody2D = character.GetComponent<Rigidbody2D>();

        raycastDistance = 0.4f;
        cooldownOfHiding = 0.7f;
        
        raycastHit2D = new RaycastHit2D();

        timerOfHiding = new SyncTimer();
        timerOfHiding.onTick += EnteringToTheHidingPlace;
        timerOfHiding.Interval = cooldownOfHiding;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        character.onTriggerEnter2D += checkingForEnteringToTheHidingPlaceMethod;
        character.onTriggerExit2D += checkingForExitingOfTheHidingPlaceMethod;
        
        Physics2D.queriesStartInColliders = false;
        canHide = false;
        EditorDebug.Log("ACTIONIDLE ENTER");
    }

    protected override void OnExit() 
    {
        base.OnExit();
        character.onTriggerEnter2D -= checkingForEnteringToTheHidingPlaceMethod;
        character.onTriggerExit2D -= checkingForExitingOfTheHidingPlaceMethod;
        character.onDetected -= onCharacterDetectedMethod;
        // EditorDebug.Log("ACTIONIDLE EXIT");
    }

    protected override void OnUpdate()
    {
        timerOfHiding.Update(Time.deltaTime);

        EditorDebug.DrawLine(character.transform.position, (Vector2)character.transform.localPosition + (Vector2)character.transform.right * raycastDistance, Color.red);

        if (character.IsMovingHorizontal)
        {
            CheckPushable();
        }

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && canHide == true && character.IsGrounded == true && !timerOfHiding.Active) 
            {
                animator.ResetTrigger("Move");
                EditorDebug.Log("LLAMADO AL TIMER ACTION");
                characterMovementFSM.Active = false;
                characterJumpingFSM.Active = false;
                characterRigidBody2D.velocity = new Vector2(0, characterRigidBody2D.velocity.y);
                timerOfHiding.Start();
                character.onDetected += onCharacterDetectedMethod;
            }
            else if(ev == Character.Order.OrderPush)
            {
                CheckPushable();
            }
            else if(ev == Character.Order.OrderAttack)
            {
                stateMachine.Trigger(Character.Trigger.Attack);
            }
        }
    }

    private void OnCharacterDetected()
    {
        Logger.AnalyticsCustomEvent("Detected_On_Hiding");
    }

    void CheckingForEnteringToTheHidingPlace(Collider2D collider2D) 
    {
        if (collider2D.gameObject.layer == Reg.hideLayer) 
        {
            canHide = true;
        }
    }

    void CheckingForExitingOfTheHidingPlace(Collider2D collider2D)
    {
        if(collider2D.gameObject.layer == Reg.hideLayer)
        {
            canHide = false;
        }
    }
    
    void EnteringToTheHidingPlace(SyncTimer timer) 
    {
        character.onDetected -= onCharacterDetectedMethod;
        Logger.AnalyticsCustomEvent("Hide_Succees");
        stateMachine.Trigger(Character.Trigger.Hide);
    }

    private void CheckPushable()
    {
        raycastHit2D = Physics2D.Raycast(character.transform.position, (Vector2)character.transform.right, raycastDistance, 1 << Reg.objectLayer);

        if (raycastHit2D && character.IsGrounded)
        {
            stateMachine.Trigger(Character.Trigger.Push);
        }

    }
}
