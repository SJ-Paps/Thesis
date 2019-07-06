using SAM.Timers;
using System;
using UnityEngine;

public class TurretIAAlertFulState : TurretIAControllerHSMState
{
    private SyncTimer calmdownTimer;

    private int accelerationConstraintId;

    private Action<Collider2D, Eyes> onTargetDetectedDelegate;

    public TurretIAAlertFulState()
    {
        calmdownTimer = new SyncTimer();
        calmdownTimer.Interval = 20f;
        calmdownTimer.onTick += OnTimerTick;

        onTargetDetectedDelegate = OnTargetDetected;

        activeDebug = true;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        float accelerationPercentage = 160;

        accelerationConstraintId = Slave.Acceleration.AddPercentageConstraint(accelerationPercentage);

        calmdownTimer.Start();

        Slave.GetEyes().onAnyStay += onTargetDetectedDelegate;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        calmdownTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        base.OnExit();

        Slave.Acceleration.RemovePercentageConstraint(accelerationConstraintId);

        Slave.GetEyes().onAnyStay -= onTargetDetectedDelegate;

        calmdownTimer.Stop();
    }

    private void OnTimerTick(SyncTimer timer)
    {
        CalmDown();
    }

    private void CalmDown()
    {
        SendEvent(TurretIAController.Trigger.CalmDown);
    }

    private void OnTargetDetected(Collider2D collider, Eyes eyes)
    {
        if(eyes.IsVisible(collider, Reg.walkableLayerMask, (1 << collider.gameObject.layer)))
        {
            Slave.SendOrder(Character.Order.Attack);
            calmdownTimer.Start();
        }
    }
}
