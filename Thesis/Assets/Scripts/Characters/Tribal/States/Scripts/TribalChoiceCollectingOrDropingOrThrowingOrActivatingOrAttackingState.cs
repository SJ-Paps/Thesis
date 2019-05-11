public class TribalChoiceCollectingOrDropingOrThrowingOrActivatingOrAttackingState : TribalHSMState
{

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
