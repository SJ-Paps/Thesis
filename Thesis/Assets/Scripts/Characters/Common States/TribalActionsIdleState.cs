using SAM.FSM;
using SAM.Timers;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class TribalActionsIdleState : CharacterState 
{
    private FSM<Character.State, Character.Trigger> characterJumpingFSM;
    private FSM<Character.State, Character.Trigger> characterMovementFSM;

    private Action<Collider2D> checkingForEnteringToTheHidingPlaceMethod;
    private Action<Collider2D> checkingForExitingOfTheHidingPlaceMethod;

    private Rigidbody2D characterRigidBody2D;
    private SyncTimer timerOfHiding;

    [SerializeField]
    private float pushCheckDistance = 0.4f, cooldownOfHiding = 0.7f;

    private bool canHide;
    private Animator animator;

    public void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard, FSM<Character.State, Character.Trigger> jumpingFSM, FSM<Character.State, Character.Trigger> movementFSM)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        characterJumpingFSM = jumpingFSM;
        characterMovementFSM = movementFSM;

        checkingForEnteringToTheHidingPlaceMethod += CheckingForEnteringToTheHidingPlace;
        checkingForExitingOfTheHidingPlaceMethod += CheckingForExitingOfTheHidingPlace;

        characterRigidBody2D = character.RigidBody2D;

        timerOfHiding = new SyncTimer();
        timerOfHiding.onTick += EnteringToTheHidingPlace;
        timerOfHiding.Interval = cooldownOfHiding;

        animator = character.Animator;
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
        EditorDebug.Log("ACTIONIDLE EXIT");
    }

    protected override void OnUpdate()
    {
        timerOfHiding.Update(Time.deltaTime);

        EditorDebug.DrawLine(character.transform.position, (Vector2)character.transform.localPosition + (Vector2)character.transform.right * pushCheckDistance, Color.red);

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
        stateMachine.Trigger(Character.Trigger.Hide);
    }

    private void CheckPushable()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(character.transform.position, (Vector2)character.transform.right, pushCheckDistance, 1 << Reg.objectLayer);

        if (raycastHit2D && character.IsGrounded)
        {
            stateMachine.Trigger(Character.Trigger.Push);
        }

    }
}
