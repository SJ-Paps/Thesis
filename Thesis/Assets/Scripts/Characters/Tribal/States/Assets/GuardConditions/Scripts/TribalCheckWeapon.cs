public class TribalCheckWeapon : TribalGuardCondition
{
    private Equipment ownerEquipment;

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        ownerEquipment = Owner.GetComponentInChildren<Equipment>();
    }

    protected override bool OnValidate()
    {
        return ownerEquipment.HasEquippedObjectOfType<Weapon>();
    }
}
