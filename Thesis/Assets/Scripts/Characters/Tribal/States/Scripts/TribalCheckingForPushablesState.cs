﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.Timers;

public class TribalCheckingForPushablesState : TribalHSMState
{
    private SyncTimer timer;

    private Vector2 checkBoxSize;

    public TribalCheckingForPushablesState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        float checkPushableInterval = 0.1f;

        timer = new SyncTimer();
        timer.Interval = checkPushableInterval;
        timer.onTick += OnTimerTick;
        timer.Loop = true;

        
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        blackboard.toPushMovableObject = null;

        checkBoxSize = new Vector2(character.Collider.bounds.extents.x / 2, character.Collider.bounds.extents.y / 2);

        timer.Start();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        timer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        base.OnExit();

        timer.Stop();
    }

    private void OnTimerTick(SyncTimer timer)
    {
        MovableObject movableObject = character.CheckForMovableObject();

        if(movableObject != null)
        {
            blackboard.toPushMovableObject = movableObject;
            SendEvent(Character.Trigger.Push);
        }
    }
}