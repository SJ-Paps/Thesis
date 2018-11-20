﻿using SAM.FSM;
using SAM.Timers;
using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class TribalHiddenState : CharacterState 
{
    private FSM<Character.State, Character.Trigger> characterJumpingFSM;
    private FSM<Character.State, Character.Trigger> characterMovementFSM;
    private SyncTimer timerForComingOut;

    [SerializeField]
    private float cooldownForComingOut = 0.7f;

    public void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard, FSM<Character.State, Character.Trigger> jumpingFSM, FSM<Character.State, Character.Trigger> movementFSM)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        characterJumpingFSM = jumpingFSM;
        characterMovementFSM = movementFSM;
        timerForComingOut = new SyncTimer();

        timerForComingOut.onTick += StopTimerForComingOut;
        timerForComingOut.Interval = cooldownForComingOut;
    }

    protected override void OnEnter() 
    {
        blackboard.isHidden = true;
        EnteringToTheHidingPlace();
        EditorDebug.Log("HIDDEN ENTER");
    }

    protected override void OnExit()
    {
        blackboard.isHidden = false;
        //  EditorDebug.Log("HIDDEN EXIT");
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

            if (ev == Character.Order.OrderHide && !timerForComingOut.Active) 
            {
                EditorDebug.Log("LLAMADO AL TIMER HIDDEN");
                timerForComingOut.Start();
            }
        }
    }

    private void EnteringToTheHidingPlace() 
    {
        characterJumpingFSM.Active = false;
        characterMovementFSM.Active = false;
    }

    private void ComingOutOfTheHidingPlace() 
    {
        characterJumpingFSM.Active = true;
        characterMovementFSM.Active = true;
        stateMachine.Trigger(Character.Trigger.StopHiding);
    }

    void StopTimerForComingOut(SyncTimer timer) 
    {
        ComingOutOfTheHidingPlace();
    }
}