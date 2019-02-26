public class ContextualActivable : ActivableObject<Tribal>
{
    public override bool Activate(Tribal user)
    {
        return true;
    }

    public override bool ShouldBeSaved()
    {
        return false;
    }
}
