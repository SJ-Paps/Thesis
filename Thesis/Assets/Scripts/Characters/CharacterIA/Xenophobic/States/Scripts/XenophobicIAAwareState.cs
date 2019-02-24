﻿using SAM.Timers;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class XenophobicIAAwareState : XenophobicIAState
{
    private SyncTimer awareTimer;

    private Action<Collider2D, Eyes> onSomethingDetectedStayDelegate;

    private float hiddenFindProbability = 30f;
    private float hiddenDetectionDistance = 1f;

    public XenophobicIAAwareState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        awareTimer = new SyncTimer();

        float awareTime = 6f;

        awareTimer.Interval = awareTime;
        awareTimer.onTick += OnTimerTick;

        onSomethingDetectedStayDelegate = AnalyzeDetection;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        awareTimer.Start();

        EyeCollection eyes = Owner.Slave.GetEyes();

        for (int i = 0; i < eyes.Count; i++)
        {
            Eyes current = eyes[i];

            int newLayerMask = Physics2D.GetLayerCollisionMask(current.Collider.gameObject.layer) | Reg.hiddenLayer;
            Physics2D.SetLayerCollisionMask(current.Collider.gameObject.layer, newLayerMask);
        }

        eyes.onAnyStay += onSomethingDetectedStayDelegate;


    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        awareTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        base.OnExit();

        EyeCollection eyes = Owner.Slave.GetEyes();

        eyes.onAnyStay -= onSomethingDetectedStayDelegate;

        awareTimer.Stop();

        for (int i = 0; i < eyes.Count; i++)
        {
            Eyes current = eyes[i];

            int newLayerMask = Physics2D.GetLayerCollisionMask(current.Collider.gameObject.layer) ^ Reg.hiddenLayer;
            Physics2D.SetLayerCollisionMask(current.Collider.gameObject.layer, newLayerMask);
        }
    }

    private void OnTimerTick(SyncTimer timer)
    {
        CalmDown();
    }

    private void AnalyzeDetection(Collider2D collider, Eyes eyes)
    {
        if (eyes.IsVisible(collider, Reg.walkableLayerMask, (1 << collider.gameObject.layer)))
        {
            if(collider.gameObject.layer == Reg.hiddenLayer)
            {
                if (Vector2.Distance(eyes.EyePoint.position, collider.transform.position) <= hiddenDetectionDistance && Random.Range(1, 100) <= hiddenFindProbability)
                {
                    UpdatePosition(collider.transform.position);

                    SetFullAlert();
                }
            }
            else
            {
                UpdatePosition(collider.transform.position);

                SetFullAlert();
            }
            
        }
    }

    private void UpdatePosition(Vector2 position)
    {
        Blackboard.LastDetectedPosition = position;
        awareTimer.Start();
    }

    private void CalmDown()
    {
        SendEvent(XenophobicIAController.Trigger.CalmDown);
    }

    private void SetFullAlert()
    {
        SendEvent(XenophobicIAController.Trigger.SetFullAlert);
    }
}
