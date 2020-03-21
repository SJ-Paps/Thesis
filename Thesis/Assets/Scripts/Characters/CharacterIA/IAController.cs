public abstract class IAController : UnityController<Character, Character.OrderEvent>
{
    public Blackboard Blackboard { get; protected set; }

    protected override void SJAwake()
    {
        base.SJAwake();

        Blackboard = GetComponent<Blackboard>();
    }
}
