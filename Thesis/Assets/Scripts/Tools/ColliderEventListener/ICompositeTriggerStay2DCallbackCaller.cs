namespace SJ.Tools
{
    public interface ICompositeTriggerStay2DCallbackCaller
    {
        void SubscribeToOnTriggerStay(IOnTriggerStay2DListener listener);
        void UnsubscribeFromOnTriggerStay(IOnTriggerStay2DListener listener);
    }
}