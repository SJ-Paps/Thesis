using System.Collections.Generic;

public class TribalCheckIsInFrontOfHide : TribalGuardCondition
{
    private List<IActivable> activables;

    public TribalCheckIsInFrontOfHide()
    {
        activables = new List<IActivable>();
    }

    protected override bool OnValidate()
    {
        return Blackboard.GetItemOf<IActivable>("Activable") is Hide || HideFound();
    }

    private bool HideFound()
    {
        activables.Clear();

        Owner.FindActivables(activables);

        activables.ContainsType<Hide>(out Hide hide);

        Blackboard.UpdateItem<IActivable>("Activable", hide);

        return hide != null;
    }
}
