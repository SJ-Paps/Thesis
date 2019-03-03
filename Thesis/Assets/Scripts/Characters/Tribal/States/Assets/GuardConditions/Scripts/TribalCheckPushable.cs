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
        return Blackboard.activable is MovableObject || MovableFound();
    }

    private bool MovableFound()
    {
        Owner.FindActivables(activables);

        activables.ContainsType<MovableObject>(out MovableObject movableObject);

        Blackboard.activable = movableObject;

        return movableObject != null;
    }
}
