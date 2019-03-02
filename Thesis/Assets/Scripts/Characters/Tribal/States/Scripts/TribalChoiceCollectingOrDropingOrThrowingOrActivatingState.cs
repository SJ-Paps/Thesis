public class TribalChoiceCollectingOrDropingOrThrowingOrActivatingState : TribalHSMState
{
    public TribalChoiceCollectingOrDropingOrThrowingOrActivatingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        if(SendEvent(LastEnteringTrigger) == false)
        {
            SendEvent(Character.Order.FinishAction);
        }
    }
}
