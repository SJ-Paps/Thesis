using UnityEngine.Analytics;
using UnityEngine;
using System.Collections.Generic;

public static partial class Logger
{
    public static void AnalyticsCustomEvent(string eventName)
    {
        EditorDebug.Log(eventName);
        InternalAnalyticsCustomEvent(eventName);
    }

    public static void AnalyticsCustomEvent(string eventName, IDictionary<string, object> eventData)
    {
        EditorDebug.Log(eventName);
        InternalAnalyticsCustomEvent(eventName, eventData);
    }

    static partial void InternalAnalyticsCustomEvent(string eventName);
    static partial void InternalAnalyticsCustomEvent(string eventName, IDictionary<string, object> eventData);
}

#if ANALYTICS

public static partial class Logger
{
    static partial void InternalAnalyticsCustomEvent(string eventName)
    {
        Analytics.CustomEvent(eventName);
    }

    static partial void InternalAnalyticsCustomEvent(string eventName, IDictionary<string, object> eventData)
    {
        Analytics.CustomEvent(eventName, eventData);
    }
}

#endif
