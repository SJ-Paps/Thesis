public abstract class IAController : UnityController<Character, Character.Order>
{
    public Blackboard Blackboard { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        Blackboard = GetComponent<Blackboard>();
    }
}
