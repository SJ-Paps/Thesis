using System.Collections.Generic;

public class TribalCheckCollectables : TribalGuardCondition
{
    private List<IActivable> activables;

    public TribalCheckCollectables()
    {
        activables = new List<IActivable>();
    }

    protected override bool OnValidate()
    {
        return (Blackboard.GetItemOf<IActivable>("Activable") is CollectableObject collectable && OwnerHasObject(collectable) == false) || CollectableFound();
    }

    private bool CollectableFound()
    {
        activables.Clear();

        Owner.FindActivables(activables);

        activables.ContainsType<CollectableObject>(out CollectableObject collectableObject);

        if(collectableObject != null && OwnerHasObject(collectableObject) == false)
        {
            Blackboard.UpdateItem<IActivable>("Activable", collectableObject);

            return true;
        }

        return false;
    }

    private bool OwnerHasObject(CollectableObject collectable)
    {
        return Owner.Inventory.Contains(collectable);
    }
}
