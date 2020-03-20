using SJ;
using UnityEngine;

public class TurretChargingState : TurretHSMState
{
    private SyncTimer timer;

    public TurretChargingState()
    {
        timer = new SyncTimer();
        timer.onTick += OnTimerTick;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        timer.Interval = Owner.ChargeTime;
        timer.Start();

        Owner.HeadRigidBody.angularVelocity = 0;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        timer.Update(Time.deltaTime);
    }

    private void OnTimerTick(SyncTimer timer)
    {
        SendEvent(Character.Order.Attack);
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Attack && timer.Active)
        {
            return true;
        }

        return false;
    }
}