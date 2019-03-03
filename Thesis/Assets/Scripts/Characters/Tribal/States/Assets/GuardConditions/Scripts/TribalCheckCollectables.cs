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
        return (Blackboard.activable != Owner.GetHand().CurrentCollectable && Blackboard.activable is CollectableObject) || CollectableFound();
    }

    private bool CollectableFound()
    {
        Owner.FindActivables(activables);

        activables.ContainsType<CollectableObject>(out CollectableObject collectableObject);

        if(collectableObject != null && collectableObject != Owner.GetHand().CurrentCollectable)
        {
            Blackboard.activable = collectableObject;

            return true;
        }

        return false;
    }
}
