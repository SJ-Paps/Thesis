public class TribalChoiceCollectingOrDropingOrThrowingOrActivatingOrAttackingState : TribalHSMState
{
    public TribalChoiceCollectingOrDropingOrThrowingOrActivatingOrAttackingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
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
