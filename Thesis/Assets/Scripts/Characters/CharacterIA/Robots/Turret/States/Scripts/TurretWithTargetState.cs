public class TurretWithTargetState : TurretIAControllerHSMState
{
    public TurretWithTargetState(TurretIAController.State state, string debugName) : base(state, debugName)
    {

    }

    /*protected override void OnEnter()
    {
        controller.Slave.SetOrder(Character.Order.OrderAttack);
        stateMachine.Trigger(TurretIAController.Trigger.TargetLost);
    }
    */
}
