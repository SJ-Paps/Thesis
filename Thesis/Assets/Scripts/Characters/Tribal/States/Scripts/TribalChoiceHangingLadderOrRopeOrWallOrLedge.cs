public class TribalChoiceHangingLadderOrRopeOrWallOrLedge : TribalHSMState
{

    protected override void OnEnter()
    {
        base.OnEnter();

        Redirect();
    }

    private void Redirect()
    {
        if(SendEvent(LastEnteringTrigger) == false)
        {
            SendEvent(Character.Order.Fall);
        }
    }
}
