public abstract class IAControllerState : SJHSMState
{
    public new IAController Owner { get; protected set; }
    protected Character Slave { get; set; }
    protected Blackboard Blackboard { get; set; }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner = (IAController)base.Owner;
        Slave = Owner.Slave;
        Blackboard = Owner.Blackboard;
    }
}
