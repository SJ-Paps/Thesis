using SAM.Timers;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class XenophobicIAAlertfulState : XenophobicIAState
{
    private SyncTimer fullAlertTimer;

    private float hiddenDetectionDistance = 1.5f;

    private float hiddenFindProbability = 80f;

    private Action<Collider2D, Eyes> onSomethingDetectedDelegate;

    public XenophobicIAAlertfulState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        float fullAlertTime = 10f;

        fullAlertTimer = new SyncTimer();
        fullAlertTimer.Interval = fullAlertTime;
        fullAlertTimer.onTick += OnTimerTick;

        onSomethingDetectedDelegate = AnalyzeDetection;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        fullAlertTimer.Start();

        EyeCollection eyes = Owner.Slave.GetEyes();

        for (int i = 0; i < eyes.Count; i++)
        {
            Eyes current = eyes[i];

            int newLayerMask = Physics2D.GetLayerCollisionMask(current.Collider.gameObject.layer) | Reg.hiddenLayer;
            Physics2D.SetLayerCollisionMask(current.Collider.gameObject.layer, newLayerMask);
        }

        eyes.onAnyStay += onSomethingDetectedDelegate;
    }

    protected override void OnExit()
    {
        base.OnExit();

        fullAlertTimer.Stop();

        EyeCollection eyes = Owner.Slave.GetEyes();

        for (int i = 0; i < eyes.Count; i++)
        {
            Eyes current = eyes[i];

            int newLayerMask = Physics2D.GetLayerCollisionMask(current.Collider.gameObject.layer) ^ Reg.hiddenLayer;
            Physics2D.SetLayerCollisionMask(current.Collider.gameObject.layer, newLayerMask);
        }

        eyes.onAnyStay -= onSomethingDetectedDelegate;
    }

    private void OnTimerTick(SyncTimer timer)
    {
        CalmDown();
    }

    private void AnalyzeDetection(Collider2D collider, Eyes eyes)
    {
        if (eyes.IsVisible(collider, Reg.walkableLayerMask, (1 << collider.gameObject.layer)))
        {
            if (collider.gameObject.layer == Reg.hiddenLayer)
            {
                if (Vector2.Distance(eyes.EyePoint.position, collider.transform.position) <= hiddenDetectionDistance && Random.Range(1, 100) <= hiddenFindProbability)
                {
                    UpdatePosition(collider.transform.position);
                }
            }
            else
            {
                UpdatePosition(collider.transform.position);
            }
        }
    }

    private void CalmDown()
    {
        SendEvent(XenophobicIAController.Trigger.CalmDown);
    }

    private void UpdatePosition(Vector2 position)
    {
        Blackboard.LastDetectedPosition = position;
        fullAlertTimer.Start();
    }

}
