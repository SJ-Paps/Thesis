public class TribalHasHandObject : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Owner.GetHand().IsFree == false;
    }
}
