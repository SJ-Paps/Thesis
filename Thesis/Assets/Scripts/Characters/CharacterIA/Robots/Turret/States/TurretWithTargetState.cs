using SAM.FSM;
using System;

[Serializable]
public class TurretWithTargetState : TurretIAState
{
    protected override void OnEnter()
    {
        controller.Slave.SetOrder(Character.Order.OrderAttack);
        stateMachine.Trigger(TurretIAController.Trigger.TargetLost);
    }

    public override void InitializeState(FSM<TurretIAController.State, TurretIAController.Trigger> stateMachine, TurretIAController.State state, TurretIAController controller, TurretIAController.Blackboard blackboard)
    {
        base.InitializeState(stateMachine, state, controller, blackboard);
    }
}
