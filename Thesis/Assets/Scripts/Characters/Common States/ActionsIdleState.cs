using SAM.FSM;
using SAM.Timers;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ActionsIdleState : CharacterState 
{
    private Action<Collider2D> checkingForEnteringToTheHidingPlaceMethod;
    private Action<Collider2D> checkingForExitingOfTheHidingPlaceMethod;
    private Collider2D characterCollider2D;
    private SyncTimer timerOfHiding;
    private float cooldownOfHiding;
    private int hidingPlaceLayer;
    private bool canHide;
    private bool cooldownReached;

    public ActionsIdleState(FSM<Character.State, Character.Trigger> fsm,
        Character.State state,
        Character character,
        List<Character.Order> orders) : base(fsm, state, character, orders)
    {
        checkingForEnteringToTheHidingPlaceMethod += CheckingForEnteringToTheHidingPlace;
        checkingForExitingOfTheHidingPlaceMethod += CheckingForExitingOfTheHidingPlace;
        timerOfHiding = new SyncTimer();
        cooldownOfHiding = 2.0f;
        hidingPlaceLayer = 8;
        canHide = false;
        cooldownReached = false;

        timerOfHiding.Interval = cooldownOfHiding;
        timerOfHiding.onTick += StopTimerOfHiding;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        timerOfHiding.Start();
        character.onTriggerEnter2D += checkingForEnteringToTheHidingPlaceMethod;
        character.onTriggerExit2D += checkingForExitingOfTheHidingPlaceMethod;
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

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && canHide == true && character.isGrounded == true) 
            {
                cooldownReached = false;
                stateMachine.Trigger(Character.Trigger.Hide);
            }
        }
    }

    void CheckingForEnteringToTheHidingPlace(Collider2D collider2D) 
    {
        if (collider2D.gameObject.layer == hidingPlaceLayer) 
        {
            canHide = true;
        }
    }

    void CheckingForExitingOfTheHidingPlace(Collider2D collider2D)
    {
        if(collider2D.gameObject.layer == hidingPlaceLayer)
        {
            canHide = false;
        }
    }
    
    void StopTimerOfHiding(SyncTimer timer) 
    {
        timer.Stop();
        cooldownReached = true;
    }
}
