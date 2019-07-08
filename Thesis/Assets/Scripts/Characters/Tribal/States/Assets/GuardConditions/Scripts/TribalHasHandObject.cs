public class TribalHasHandObject : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Owner.Equipment.HasSomethingEquipped();
    }
}
