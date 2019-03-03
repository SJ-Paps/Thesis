
public enum ActivableState : byte
{
    Off,
    On,
}

public interface IActivable
{
    ActivableState State { get; }

    bool Activate(object user);
}

public interface IActivable<in TActivator> : IActivable where TActivator : class
{
    bool Activate(TActivator user);
}
