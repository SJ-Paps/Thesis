using System;
using SAM.FSM;
using UnityEngine;

public class TurretAlertlessState : TurretIAState
{
    private float movementVelocity;

    private float previousVelocity;

    private Action<Vector2> onTargetUpdatedDelegate;

    public TurretAlertlessState(TurretIAController.State state, string debugName) : base(state, debugName)
    {
        //onTargetUpdatedDelegate = OnTargetUpdated;
    }

    /*protected override void OnEnter()
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

    private void OnTargetUpdated(Vector2 position)
    {
        stateMachine.Trigger(TurretIAController.Trigger.SetFullAlert);
    }*/
}
