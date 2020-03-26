namespace SJ.Management
{
    public interface ICompositeUpdatable : IUpdateListener, ILateUpdateListener, IFixedUpdateListener
    {
        void SubscribeToUpdate(IUpdateListener updatable);
        void SubscribeToLateUpdate(ILateUpdateListener updatable);
        void SubscribeToFixedUpdate(IFixedUpdateListener updatable);

        void UnsubscribeFromUpdate(IUpdateListener updatable);
        void UnsubscribeFromLateUpdate(ILateUpdateListener updatable);
        void UnsubscribeFromFixedUpdate(IFixedUpdateListener updatable);
    }
}


