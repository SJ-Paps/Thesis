public class TribalCheckBlackboardContainsLadder : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Blackboard.GetItemOf<IActivable>("Activable") is Ladder;
    }
}
