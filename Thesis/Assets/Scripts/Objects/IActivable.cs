
public interface IActivable<TActivator> where TActivator : class
{
    bool Activate(TActivator user);
}
