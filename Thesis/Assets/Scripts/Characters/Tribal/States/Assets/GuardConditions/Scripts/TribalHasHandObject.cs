public class TribalHasHandObject : TribalGuardCondition
{
    private Equipment ownerEquipment;

    protected override bool OnValidate()
    {
        return ownerEquipment.HasSomethingEquipped();
    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        ownerEquipment = Owner.GetComponentInChildren<Equipment>();
    }
}
