public class Hide : ActivableObject<Character> {

    public override void Activate(Character user)
    {
        base.Activate(user);
    }

    public override bool ShouldBeSaved()
    {
        return false;
    }
}
