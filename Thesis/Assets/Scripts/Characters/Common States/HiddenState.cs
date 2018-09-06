using SAM.FSM;
using SAM.Timers;
using System;
using UnityEngine;
using System.Collections.Generic;

public class HiddenState : CharacterState 
{
    private FSM<Character.State, Character.Trigger> characterJumpingFSM;
    private FSM<Character.State, Character.Trigger> characterMovementFSM;
    private Rigidbody2D characterRigidbody2D;
    private Collider2D characterCollider2D;
    private SyncTimer timerForComingOut;
    private Character.Blackboard characterBlackboard;
    private int hidingPlaceLayer;
    private float cooldownForComingOut;
    private bool isComingOut;

    public HiddenState(FSM<Character.State, Character.Trigger> fsm, 
       Character.State state,
       Character character,
       List<Character.Order> orders,
       Character.Blackboard blackboard,
       FSM<Character.State,Character.Trigger> jumpingFSM,
       FSM<Character.State, Character.Trigger> movementFSM) : base(fsm, state, character, orders)
    {
        characterJumpingFSM = jumpingFSM;
        characterMovementFSM = movementFSM;
        characterRigidbody2D = character.GetComponent<Rigidbody2D>();
        characterCollider2D = character.GetComponent<Collider2D>();
        timerForComingOut = new SyncTimer();
        characterBlackboard = blackboard;
        hidingPlaceLayer = 8;
        cooldownForComingOut = 2.0f;
        isComingOut = false;

        timerForComingOut.Interval = cooldownForComingOut;
        timerForComingOut.onTick += StopTimerForComingOut;
    }

    protected override void OnEnter() 
    {
        characterBlackboard.isHidden = true;
        EnteringToTheHidingPlace();
        EditorDebug.Log("HIDDEN ENTER");
    }

    protected override void OnExit()
    {
        characterBlackboard.isHidden = false;
        EditorDebug.Log("HIDDEN EXIT");
    }

    protected override void OnUpdate() 
    {
        Hide();
    }

    private void Hide() 
    {
        timerForComingOut.Update(Time.deltaTime);

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && character.isHidden == true) 
            {
                timerForComingOut.Start();
            }
            if(isComingOut)
            {
                ComingOutOfTheHidingPlace();
            }
        }
    }

    private void EnteringToTheHidingPlace() 
    {
        characterCollider2D.isTrigger = true;
        characterRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        characterJumpingFSM.Active = false;
        characterMovementFSM.Active = false;
    }

    private void ComingOutOfTheHidingPlace() 
    {
        isComingOut = false;
        characterCollider2D.isTrigger = false;
        characterRigidbody2D.constraints = RigidbodyConstraints2D.None;
        characterJumpingFSM.Active = true;
        characterMovementFSM.Active = true;
        stateMachine.Trigger(Character.Trigger.StopHiding);
    }

    void StopTimerForComingOut(SyncTimer timer) 
    {
        isComingOut = true;
        timerForComingOut.Stop();
    }
}