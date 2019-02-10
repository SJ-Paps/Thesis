
public interface IActivable<TActivator> where TActivator : class
{
    void Activate(TActivator user);
}
