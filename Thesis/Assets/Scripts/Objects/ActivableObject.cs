public abstract class ActivableObject<TActivator> : SJMonoBehaviourSaveable, IActivable<TActivator> where TActivator : class
{
    public abstract bool Activate(TActivator user);
}
