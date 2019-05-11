public class TribalCheckWeapon : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        return Owner.GetHand().IsFree == false && Owner.GetHand().CurrentCollectable is Weapon;
    }
}
