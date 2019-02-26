
public interface IActivable<in TActivator> where TActivator : class
{
    bool Activate(TActivator user);
}
