public class TribalCheckBlackboardContainsWall : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Blackboard.GetItemOf<IActivable>("Activable") is ClimbableWall;
    }
}
