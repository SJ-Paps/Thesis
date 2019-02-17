public class ContextualActivable : ActivableObject<Tribal>
{
    public override bool ShouldBeSaved()
    {
        return false;
    }
}
