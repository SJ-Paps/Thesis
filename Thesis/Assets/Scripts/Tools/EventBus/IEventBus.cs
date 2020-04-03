using System;

namespace Paps.EventBus
{
    public interface IEventBus
    {
        void Publish(string topic);
        void Publish(string topic, object data);
        void Subscribe(string topic, Action callback);
        void Subscribe(string topic, Action<object> callback);
        void Unsubscribe(string topic, Action callback);
        void Unsubscribe(string topic, Action<object> callback);
    }
}