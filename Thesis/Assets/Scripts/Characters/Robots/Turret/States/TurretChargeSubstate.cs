using System.Collections;
using System.Collections.Generic;
using SAM.FSM;
using UnityEngine;
using SAM.Timers;
using System;

[Serializable]
public class TurretChargeSubstate : TurretAttackSubstate
{
    private SyncTimer chargeTimer;

    [SerializeField]
    private float chargeTime;

    public override void InitializeState(FSM<TurretAttackState.State, TurretAttackState.Trigger> stateMachine, TurretAttackState.State state, Character character, Character.Blackboard blackboard)
    {
        base.InitializeState(stateMachine, state, character, blackboard);

        chargeTimer = new SyncTimer();
        chargeTimer.Interval = chargeTime;
        chargeTimer.onTick += Attack;
    }

    protected override void OnEnter()
    {
        chargeTimer.Start();
    }

    protected override void OnUpdate()
    {
        chargeTimer.Update(Time.deltaTime);
    }

    private void Attack(SyncTimer timer)
    {
        stateMachine.Trigger(TurretAttackState.Trigger.GoNext);
    }
    
}
