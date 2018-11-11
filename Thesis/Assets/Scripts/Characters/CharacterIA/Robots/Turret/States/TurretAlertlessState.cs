using System;
using SAM.FSM;
using UnityEngine;

[Serializable]
public class TurretAlertlessState : TurretIAState
{
    [SerializeField]
    private float movementVelocity;

    private float previousVelocity;

    private Action<Vector2> onTargetUpdatedDelegate;

    protected override void OnEnter()
    {
        previousVelocity = controller.Slave.MovementVelocity;
        controller.Slave.MovementVelocity = movementVelocity;

        blackboard.onTargetChanged += onTargetUpdatedDelegate;
    }

    protected override void OnExit()
    {
        controller.Slave.MovementVelocity = previousVelocity;

        blackboard.onTargetChanged -= onTargetUpdatedDelegate;
    }

    public override void InitializeState(FSM<TurretIAController.State, TurretIAController.Trigger> stateMachine, TurretIAController.State state, TurretIAController controller, TurretIAController.Blackboard blackboard)
    {
        base.InitializeState(stateMachine, state, controller, blackboard);

        onTargetUpdatedDelegate = OnTargetUpdated;
    }

    private void OnTargetUpdated(Vector2 position)
    {
        stateMachine.Trigger(TurretIAController.Trigger.SetFullAlert);
    }
}
