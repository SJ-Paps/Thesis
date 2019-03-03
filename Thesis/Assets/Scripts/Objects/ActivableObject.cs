
public abstract class ActivableObject<TActivator> : SJMonoBehaviour, IActivable<TActivator> where TActivator : class
{
    public ActivableState State { get; protected set; }

    public bool Activate(object user)
    {
        if(user is TActivator activator && ValidateActivation())
        {
            return Activate(user);
        }

        return false;
    }

    public abstract bool Activate(TActivator user);

    protected virtual bool ValidateActivation()
    {
        return true;
    }
}
