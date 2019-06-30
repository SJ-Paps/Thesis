public class TribalHasHandObject : TribalGuardCondition
{
    private Equipment ownerEquipment;

    protected override bool OnValidate()
    {
        return ownerEquipment.HasSomethingEquipped();
    }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        ownerEquipment = Configuration.Equipment;
    }
}
