namespace SJ.Tools
{
    public interface ICompositeCollisionExit2DCallbackCaller
    {
        void SubscribeToOnCollisionExit(IOnCollisionExit2DListener listener);
        void UnsubscribeFromOnCollisionExit(IOnCollisionExit2DListener listener);
    }
}