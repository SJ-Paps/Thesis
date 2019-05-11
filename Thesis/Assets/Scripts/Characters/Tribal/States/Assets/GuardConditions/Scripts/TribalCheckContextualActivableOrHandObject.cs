using System.Collections.Generic;

public class TribalCheckContextualActivableOrHandObject : TribalGuardCondition
{
    private List<IActivable> activables;

    public TribalCheckContextualActivableOrHandObject()
    {
        activables = new List<IActivable>();
    }

    protected override bool OnValidate()
    {
        return Blackboard.activable is ContextualActivable || ContextualActivableFound() || Owner.GetHand().IsFree == false;
    }

    private bool ContextualActivableFound()
    {
        activables.Clear();

        Owner.FindActivables(activables);

        activables.ContainsType<ContextualActivable>(out ContextualActivable contextualActivable);

        Blackboard.activable = contextualActivable;

        return contextualActivable != null;
    }
}
