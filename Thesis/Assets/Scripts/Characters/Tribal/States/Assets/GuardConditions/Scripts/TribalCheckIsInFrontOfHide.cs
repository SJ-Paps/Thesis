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
        return Blackboard.activable is Hide || HideFound();
    }

    private bool HideFound()
    {
        activables.Clear();

        Owner.FindActivables(activables);

        activables.ContainsType<Hide>(out Hide hide);

        Blackboard.activable = hide;

        return hide != null;
    }
}
