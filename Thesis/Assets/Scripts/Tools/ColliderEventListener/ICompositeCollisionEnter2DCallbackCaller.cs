namespace SJ.Tools
{
    public interface ICompositeCollisionEnter2DCallbackCaller
    {
        void SubscribeToOnCollisionEnter(IOnCollisionEnter2DListener listener);
        void UnsubscribeFromOnCollisionEnter(IOnCollisionEnter2DListener listener);
    }
}