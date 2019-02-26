public class Hide : ActivableObject<Tribal> {

    public override bool Activate(Tribal user)
    {
        base.Activate(user);

        return true;
    }

    public override bool ShouldBeSaved()
    {
        return false;
    }
}
