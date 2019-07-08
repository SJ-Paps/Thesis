public class TribalCheckWeapon : TribalGuardCondition
{

    protected override bool OnValidate()
    {
        return Owner.Equipment.HasEquippedObjectOfType<Weapon>();
    }
}
