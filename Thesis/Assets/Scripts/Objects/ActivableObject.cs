
public abstract class ActivableObject<TActivator> : SJMonoBehaviour, IActivable<TActivator> where TActivator : class
{
    public bool Active { get; protected set; }

    public bool Activate(object user)
    {
        if(user is TActivator activator)
        {
            return Activate(user);
        }

        return false;
    }

    public abstract bool Activate(TActivator user);
}
