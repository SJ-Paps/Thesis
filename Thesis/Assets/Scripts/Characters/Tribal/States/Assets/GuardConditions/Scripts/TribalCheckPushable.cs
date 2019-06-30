using System.Collections.Generic;

public class TribalCheckPushable : TribalGuardCondition
{
    private List<IActivable> activables;

    public TribalCheckPushable()
    {
        activables = new List<IActivable>();
    }

    protected override bool OnValidate()
    {
        return Blackboard.GetItemOf<IActivable>("Activable") is MovableObject || MovableFound();
    }

    private bool MovableFound()
    {
        activables.Clear();

        Owner.FindActivables(activables);

        activables.ContainsType<MovableObject>(out MovableObject movableObject);

        Blackboard.UpdateItem<IActivable>("Activable", movableObject);

        return movableObject != null;
    }
}
