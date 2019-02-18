public class Hide : ActivableObject<Tribal> {

    public override void Activate(Tribal user)
    {
        base.Activate(user);
    }

    public override bool ShouldBeSaved()
    {
        return false;
    }
}
