using System;
using System.Collections.Generic;

namespace Paps.EventBus
{
    public class EventBus : IEventBus
    {
        private class Callbacks
        {
            public int DataCallbackCount => DataCallbacks.GetInvocationList().Length;
            public int NoDataCallbackCount => NoDataCallbacks.GetInvocationList().Length;

            public Action NoDataCallbacks;
            public Action<object> DataCallbacks;
        }

        private Dictionary<string, Callbacks> topicCallbacks = new Dictionary<string, Callbacks>();

        public void Publish(string topic)
        {
            if (topicCallbacks.ContainsKey(topic))
                topicCallbacks[topic].NoDataCallbacks();
        }

        public void Publish(string topic, object data)
        {
            if (topicCallbacks.ContainsKey(topic))
                topicCallbacks[topic].DataCallbacks(data);
        }

        public void Subscribe(string topic, Action callback)
        {
            if (topicCallbacks.ContainsKey(topic) == false)
                topicCallbacks.Add(topic, new Callbacks());

            var callbacks = topicCallbacks[topic];

            callbacks.NoDataCallbacks += callback;
        }

        public void Subscribe(string topic, Action<object> callback)
        {
            if (topicCallbacks.ContainsKey(topic) == false)
                topicCallbacks.Add(topic, new Callbacks());

            var callbacks = topicCallbacks[topic];

            callbacks.DataCallbacks += callback;
        }

        public void Unsubscribe(string topic, Action callback)
        {
            if(topicCallbacks.ContainsKey(topic))
            {
                var callbacks = topicCallbacks[topic];

                callbacks.NoDataCallbacks -= callback;

                if (callbacks.NoDataCallbackCount == 0 && callbacks.DataCallbackCount == 0)
                    topicCallbacks.Remove(topic);
            }
        }

        public void Unsubscribe(string topic, Action<object> callback)
        {
            if (topicCallbacks.ContainsKey(topic))
            {
                var callbacks = topicCallbacks[topic];

                callbacks.DataCallbacks -= callback;

                if (callbacks.NoDataCallbackCount == 0 && callbacks.DataCallbackCount == 0)
                    topicCallbacks.Remove(topic);
            }
        }
    }
}