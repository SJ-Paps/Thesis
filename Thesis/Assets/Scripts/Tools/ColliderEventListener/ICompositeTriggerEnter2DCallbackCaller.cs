namespace SJ.Tools
{
    public interface ICompositeTriggerEnter2DCallbackCaller
    {
        void SubscribeToOnTriggerEnter(IOnTriggerEnter2DListener listener);
        void UnsubscribeFromOnTriggerEnter(IOnTriggerEnter2DListener listener);
    }
}