namespace SJ.Updatables
{
    public interface IUpdater
    {
        bool IsActive { get; }

        void SubscribeToUpdate(IUpdatable updatable);
        void UnsubscribeFromUpdate(IUpdatable updateable);
        void SubscribeToLateUpdate(IUpdatable updatable);
        void UnsubscribeFromLateUpdate(IUpdatable updateable);
        void SubscribeToFixedUpdate(IUpdatable updatable);
        void UnsubscribeFromFixedUpdate(IUpdatable updateable);
        void Enable();
        void Disable();
    }
}