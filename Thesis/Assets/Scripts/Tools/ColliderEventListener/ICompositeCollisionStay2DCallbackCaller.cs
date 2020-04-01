namespace SJ.Tools
{
    public interface ICompositeCollisionStay2DCallbackCaller
    {
        void SubscribeToOnCollisionStay(IOnCollisionStay2DListener listener);
        void UnsubscribeFromOnCollisionStay(IOnCollisionStay2DListener listener);
    }
}