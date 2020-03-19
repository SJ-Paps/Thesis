using SJ;

public abstract class ActivableObject<TActivator> : SJMonoBehaviour, IActivable<TActivator> where TActivator : class
{
    public bool Active { get; protected set; }

    public abstract bool Activate(TActivator user);
}
