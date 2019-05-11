public class TribalCheckBlackboardContainsRope : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Blackboard.activable is Rope;
    }
}
