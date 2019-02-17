public class RobotHSMState : CharacterHSMState
{
    protected new Robot character;

    public RobotHSMState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        character = (Robot)base.Owner;
    }
}