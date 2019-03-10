public class TribalCheckBlackboardContainsWall : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Blackboard.activable is ClimbableWall;
    }
}
