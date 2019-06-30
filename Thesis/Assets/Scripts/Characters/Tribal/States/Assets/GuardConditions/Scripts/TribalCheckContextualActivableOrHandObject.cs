using System.Collections.Generic;

public class TribalCheckContextualActivableOrHandObject : TribalGuardCondition
{
    private List<IActivable> activables;

    private BlackboardNode<IActivable> activableNode;

    public TribalCheckContextualActivableOrHandObject()
    {
        activables = new List<IActivable>();
    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        activableNode = Blackboard.GetItemNodeOf<IActivable>("Activable");
    }

    protected override bool OnValidate()
    {
        return Blackboard.GetItemOf<IActivable>("Activable") is ContextualActivable || ContextualActivableFound();
    }

    private bool ContextualActivableFound()
    {
        activables.Clear();

        Owner.FindActivables(activables);

        activables.ContainsType<ContextualActivable>(out ContextualActivable contextualActivable);
        
        activableNode.SetValue(contextualActivable);

        return contextualActivable != null;
    }
}
