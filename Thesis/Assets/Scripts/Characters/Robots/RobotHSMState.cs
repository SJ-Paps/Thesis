public class RobotHSMState : CharacterHSMState
{
    protected new Robot Owner { get; set; }

    public RobotHSMState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (Robot)base.Owner;
    }
}