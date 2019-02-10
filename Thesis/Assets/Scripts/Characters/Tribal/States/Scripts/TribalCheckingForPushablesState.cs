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
        Vector2 detectionSize = new Vector2((character.Collider.bounds.extents.x * 2) + Tribal.movableObjectDetectionOffset, character.Collider.bounds.extents.y * 2);

        MovableObject movableObject = SJUtil.FindActivable<MovableObject, Character>(character.Collider.bounds.center, detectionSize, character.transform.eulerAngles.z);

        if (movableObject != null)
        {
            blackboard.toPushMovableObject = movableObject;
            SendEvent(Character.Trigger.Push);
        }
    }
}