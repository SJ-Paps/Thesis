public class TribalCheckPushable : TribalGuardCondition
{
    protected override bool Validate()
    {
        return IsPushableReachable();
    }

    private bool IsPushableReachable()
    {
        return Owner.CheckForMovableObject() != null;
    }
}
