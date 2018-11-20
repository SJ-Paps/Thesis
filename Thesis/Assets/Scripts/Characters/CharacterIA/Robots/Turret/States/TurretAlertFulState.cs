using System;
using UnityEngine;
using SAM.Timers;
using SAM.FSM;

[Serializable]
public class TurretAlertFulState : TurretIAState
{
    [SerializeField]
    private float movementVelocity;

    private float previousVelocity;

    private Action<Vector2> onTargetUpdatedDelegate;

    private SyncTimer calmDownTimer;

    [SerializeField]
    private float timeUntilCalmDown;

    protected override void OnEnter()
    {
        previousVelocity = controller.Slave.MovementVelocity;
        controller.Slave.MovementVelocity = movementVelocity;
    }

    protected override void OnUpdate()
    {
        calmDownTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        controller.Slave.MovementVelocity = previousVelocity;
    }

    public override void InitializeState(FSM<TurretIAController.State, TurretIAController.Trigger> stateMachine, TurretIAController.State state, TurretIAController controller, TurretIAController.Blackboard blackboard)
    {
        base.InitializeState(stateMachine, state, controller, blackboard);

        calmDownTimer = new SyncTimer();
        calmDownTimer.Interval = timeUntilCalmDown;
        calmDownTimer.onTick += CalmDown;
    }

    private void CalmDown(SyncTimer timer)
    {
        stateMachine.Trigger(TurretIAController.Trigger.CalmDown);
    }

    private void OnTargetUpdated(Vector2 position)
    {
        calmDownTimer.Start();
    }
}
