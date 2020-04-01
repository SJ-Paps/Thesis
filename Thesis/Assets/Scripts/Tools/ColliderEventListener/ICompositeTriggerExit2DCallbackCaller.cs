namespace SJ.Tools
{
    public interface ICompositeTriggerExit2DCallbackCaller
    {
        void SubscribeToOnTriggerExit(IOnTriggerExit2DListener listener);
        void UnsubscribeFromOnTriggerExit(IOnTriggerExit2DListener listener);
    }
}