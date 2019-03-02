public class TribalChoiceIsMovingByWillOrBrakingState : TribalHSMState
{
    public TribalChoiceIsMovingByWillOrBrakingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        if(LastEnteringTrigger == Character.Order.MoveLeft || LastEnteringTrigger == Character.Order.MoveRight)
        {
            SendEvent(Character.Order.Move);
        }
        else
        {
            SendEvent(Character.Order.StopMoving);
        }
    }
}