public class TribalCheckBlackboardContainsRope : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Blackboard.GetItemOf<IActivable>("Activable") is Rope;
    }
}
