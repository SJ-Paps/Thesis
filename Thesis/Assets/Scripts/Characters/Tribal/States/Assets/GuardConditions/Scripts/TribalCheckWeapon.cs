public class TribalCheckWeapon : TribalGuardCondition
{
    private Equipment ownerEquipment;

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        ownerEquipment = Configuration.Equipment;
    }

    protected override bool OnValidate()
    {
        return ownerEquipment.HasEquippedObjectOfType<Weapon>();
    }
}
