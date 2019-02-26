
public interface IActivable
{
    bool Activate(object user);
}

public interface IActivable<in TActivator> : IActivable where TActivator : class
{
    bool Activate(TActivator user);
}
