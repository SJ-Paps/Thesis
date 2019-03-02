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
        EditorDebug.Log(LastEnteringTrigger);

        if(LastEnteringTrigger == Character.Order.Collect
            || LastEnteringTrigger == Character.Order.Drop
            || LastEnteringTrigger == Character.Order.Throw)
        {
            SendEvent(LastEnteringTrigger);
        }
        else
        {
            SendEvent(Character.Order.Activate);
        }
    }
}
