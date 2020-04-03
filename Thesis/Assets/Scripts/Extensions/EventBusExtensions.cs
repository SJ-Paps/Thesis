using Paps.EventBus;
using System;
using SJ.Management;

public static class EventBusExtensions
{
    public static void Subscribe(this IEventBus eventBus, ApplicationEvents topic, Action callback)
    {
        eventBus.Subscribe(topic.ToString(), callback);
    }

    public static void Subscribe(this IEventBus eventBus, ApplicationEvents topic, Action<object> callback)
    {
        eventBus.Subscribe(topic.ToString(), callback);
    }

    public static void Unsubscribe(this IEventBus eventBus, ApplicationEvents topic, Action callback)
    {
        eventBus.Unsubscribe(topic.ToString(), callback);
    }

    public static void Unsubscribe(this IEventBus eventBus, ApplicationEvents topic, Action<object> callback)
    {
        eventBus.Unsubscribe(topic.ToString(), callback);
    }

    public static void Publish(this IEventBus eventBus, ApplicationEvents topic)
    {
        eventBus.Publish(topic.ToString());
    }

    public static void Publish(this IEventBus eventBus, ApplicationEvents topic, object data)
    {
        eventBus.Publish(topic.ToString(), data);
    }
}