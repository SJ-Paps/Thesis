namespace SJ.Management
{
    public interface IUpdater
    {
        bool IsActive { get; }

        void SubscribeToUpdate(IUpdateListener updatable);
        void UnsubscribeFromUpdate(IUpdateListener updateable);
        void SubscribeToLateUpdate(ILateUpdateListener updatable);
        void UnsubscribeFromLateUpdate(ILateUpdateListener updateable);
        void SubscribeToFixedUpdate(IFixedUpdateListener updatable);
        void UnsubscribeFromFixedUpdate(IFixedUpdateListener updateable);
        void Enable();
        void Disable();
    }
}