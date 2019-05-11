public interface IActivable
{
    bool Active { get; }
}

public interface IActivable<in TActivator> : IActivable where TActivator : class
{
    bool Activate(TActivator user);
}
