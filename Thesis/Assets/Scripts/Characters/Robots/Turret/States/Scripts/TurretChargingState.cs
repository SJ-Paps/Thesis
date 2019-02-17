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

        timer.Interval = character.ChargeTime.CurrentValueFloat;
        timer.Start();
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
}