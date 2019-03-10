public class TribalChoiceWalkingOrTrottingOrRunningState : TribalHSMState
{
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