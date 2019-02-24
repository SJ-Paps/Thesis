using SAM.Timers;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class XenophobicIAAlertState : XenophobicIAState
{
    private Action<Collider2D, Eyes> onSomethingDetectedDelegate;

    private SyncTimer unnerveTimer;

    private SyncTimer analyzeDetectionTimer;
    private float analyzeDetectionTime = 1.5f;

    private Collider2D lastDetectedCollider;
    private Eyes lastDetectorEyes;

    public XenophobicIAAlertState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        unnerveTimer = new SyncTimer();
        unnerveTimer.onTick += OnUnnerveTimerTick;
        unnerveTimer.Loop = true;

        analyzeDetectionTimer = new SyncTimer();
        analyzeDetectionTimer.Interval = analyzeDetectionTime;
        analyzeDetectionTimer.onTick += OnAnalyzeDetectionTimerTick;

        onSomethingDetectedDelegate = OnSomethingDetected;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Slave.GetEyes().onAnyStay += onSomethingDetectedDelegate;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        unnerveTimer.Update(Time.deltaTime);
        analyzeDetectionTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Slave.GetEyes().onAnyStay -= onSomethingDetectedDelegate;

        unnerveTimer.Stop();
    }

    private void OnSomethingDetected(Collider2D collider, Eyes eyes)
    {
        if(analyzeDetectionTimer.Active == false)
        {
            lastDetectedCollider = collider;
            lastDetectorEyes = eyes;

            analyzeDetectionTimer.Start();
        }
    }

    private bool HasBeenDetected(Collider2D collider, Eyes eyes)
    {
        if (eyes.IsVisible(collider, Reg.walkableLayerMask, (1 << collider.gameObject.layer)))
        {
            if (collider.gameObject.layer == Reg.hiddenLayer)
            {
                if (Vector2.Distance(eyes.EyePoint.position, collider.transform.position) <= Owner.CurrentHiddenDetectionDistance
                    && Random.Range(1, 100) <= Owner.CurrentHiddenDetectionProbability)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    private void UpdateTargetPosition(Vector2 position)
    {
        Blackboard.LastDetectedPosition = position;

        Unnerve();

        unnerveTimer.Stop();
        unnerveTimer.Interval = Owner.CurrentSeekingTime;
        unnerveTimer.Start();
    }

    private void OnUnnerveTimerTick(SyncTimer timer)
    {
        CalmDown();
    }

    private void OnAnalyzeDetectionTimerTick(SyncTimer timer)
    {
        if(HasBeenDetected(lastDetectedCollider, lastDetectorEyes))
        {
            UpdateTargetPosition(lastDetectedCollider.transform.position);
        }
    }

    private void Unnerve()
    {
        SendEvent(XenophobicIAController.Trigger.Unnerve);
    }

    private void CalmDown()
    {
        SendEvent(XenophobicIAController.Trigger.CalmDown);
    }
}
