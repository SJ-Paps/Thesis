using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.Timers;

public class TribalCheckingForPushablesState : CharacterHSMState
{
    private SyncTimer timer;

    public TribalCheckingForPushablesState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        float checkPushableInterval = 0.1f;

        timer = new SyncTimer();
        timer.Interval = checkPushableInterval;
        timer.onTick += OnTimerTick;
        timer.Loop = true;

        activeDebug = true;

        
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        blackboard.toPushMovableObject = null;

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
        CheckPushable();
    }

    private void CheckPushable()
    {
        float pushCheckDistance = 0.2f;

        MovableObject movableObject;

        if(character.IsMovableObjectNear(pushCheckDistance, out movableObject))
        {
            blackboard.toPushMovableObject = movableObject;
            SendEvent(Character.Trigger.Push);
            
        }
    }
}