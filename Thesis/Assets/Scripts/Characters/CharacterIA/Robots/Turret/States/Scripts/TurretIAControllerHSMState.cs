public class TurretIAControllerHSMState : SJHSMState<TurretIAController.State, TurretIAController.Trigger>
{
    public new TurretIAController Owner { get; private set; }
    protected new TurretIAController.Blackboard Blackboard { get; private set; }

    public TurretIAControllerHSMState(TurretIAController.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (TurretIAController)base.Owner;
        Blackboard = (TurretIAController.Blackboard)base.Blackboard;
    }
}
