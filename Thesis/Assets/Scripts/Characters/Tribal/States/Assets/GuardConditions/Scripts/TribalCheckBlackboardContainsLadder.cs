public class TribalCheckBlackboardContainsLadder : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Blackboard.activable is Ladder;
    }
}
