namespace SJ.Updatables
{
    public interface ICompositeUpdatable : IUpdatable
    {
        void SubscribeToUpdate(IUpdatable updatable);
        void SubscribeToLateUpdate(IUpdatable updatable);
        void SubscribeToFixedUpdate(IUpdatable updatable);

        void UnsubscribeFromUpdate(IUpdatable updatable);
        void UnsubscribeFromLateUpdate(IUpdatable updatable);
        void UnsubscribeFromFixedUpdate(IUpdatable updatable);
    }
}


