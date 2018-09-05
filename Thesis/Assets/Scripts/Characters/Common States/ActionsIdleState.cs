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
    private Rigidbody2D characterRigidbody2D;
    private Character.Blackboard characterBlackboard;
    private Collider2D characterCollider2D;
    private int hidingPlaceLayer;
    //private SyncTimer syncTimer;

    public ActionsIdleState(FSM<Character.State, Character.Trigger> fsm,
        Character.State state,
        Character character,
        List<Character.Order> orders,
        Character.Blackboard blackboard,
        FSM<Character.State, Character.Trigger> jumpingFSM,
        FSM<Character.State, Character.Trigger> movementFSM) : base(fsm, state, character, orders)
    {
        characterJumpingFSM = jumpingFSM;
        characterMovementFSM = movementFSM;
        characterBlackboard = blackboard;
        hidingPlaceLayer = 8;
        characterCollider2D = character.GetComponent<Collider2D>();
        checkingForEnteringToTheHidingPlaceMethod += CheckingForEnteringToTheHidingPlace;
        characterRigidbody2D = character.GetComponent<Rigidbody2D>();
        //syncTimer.Interval = 2.0f;
        //syncTimer.onTick += sfadff;
        //syncTimer.Start();
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        EditorDebug.Log("Entrado a ActionIdle");
        character.onTriggerEnter2D += checkingForEnteringToTheHidingPlaceMethod;
    }

    protected override void OnExit() {
        base.OnExit();
        EditorDebug.Log("Salí de ActionIdle");
        character.onTriggerEnter2D -= checkingForEnteringToTheHidingPlaceMethod;
    }

    protected override void OnUpdate()
    {
        //syncTimer.Update(Time.deltaTime);
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && character.isHidden == true /*&& actualCooldownToHide >= necessaryCooldownToHide*/) 
            {
                EnteringToTheHidingPlace();
            }
        }
    }

    //void sfadff(SyncTimer pacha) {
    //    pacha.Stop();
    //}

    void CheckingForEnteringToTheHidingPlace(Collider2D collider2D) 
    {
        if (collider2D.gameObject.layer == hidingPlaceLayer) 
        {
            characterBlackboard.isHiding = true;
        }
    }

    private void EnteringToTheHidingPlace() 
    {
        //EditorDebug.Log(character.isHidden);
        characterCollider2D.isTrigger = true;
        characterRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        characterBlackboard.isHiding = true;
        characterJumpingFSM.Active = false;
        characterMovementFSM.Active = false;
        stateMachine.Trigger(Character.Trigger.Hide);
    }
}
