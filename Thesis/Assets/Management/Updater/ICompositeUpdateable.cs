namespace SJ.Management
{
    public interface ICompositeUpdateable : IUpdateListener, ILateUpdateListener, IFixedUpdateListener
    {
        void SubscribeToUpdate(IUpdateListener listener);
        void SubscribeToLateUpdate(ILateUpdateListener listener);
        void SubscribeToFixedUpdate(IFixedUpdateListener listener);

        void UnsubscribeFromUpdate(IUpdateListener listener);
        void UnsubscribeFromLateUpdate(ILateUpdateListener listener);
        void UnsubscribeFromFixedUpdate(IFixedUpdateListener listener);
    }
}


