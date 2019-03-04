public class TribalChoiceWalkingOrTrottingOrRunningState : TribalHSMState
{
    public TribalChoiceWalkingOrTrottingOrRunningState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        SendEvent(Character.Order.Trot);
    }
}