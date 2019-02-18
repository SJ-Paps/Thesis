using SAM.Timers;
using UnityEngine;

public class TurretChargingState : TurretHSMState
{
    private SyncTimer timer;

    public TurretChargingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        timer = new SyncTimer();
        timer.onTick += OnTimerTick;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        timer.Interval = Owner.ChargeTime.CurrentValueFloat;
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
        SendEvent(Character.Trigger.Attack);
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Attack && timer.Active)
        {
            return true;
        }

        return false;
    }
}