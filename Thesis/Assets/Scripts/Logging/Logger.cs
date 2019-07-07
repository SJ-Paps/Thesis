using UnityEngine.Analytics;
using UnityEngine;
using System.Collections.Generic;

public static partial class Logger
{
    public static void LogConsole(object obj)
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD) && LOG
        Debug.Log(obj);
#endif
    }

    public static void LogWarning(object obj)
    {
#if UNITY_EDITOR && LOG
        Debug.LogWarning(obj);
#endif
    }

    public static void LogError(object obj)
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.LogError(obj);
#endif
    }

    public static void DrawLine(Vector3 origin, Vector3 end, Color color)
    {
#if UNITY_EDITOR && LOG
        Debug.DrawLine(origin, end, color);
#endif
    }

    public static void AnalyticsCustomEvent(string eventName)
    {
        Logger.LogConsole(eventName);
        InternalAnalyticsCustomEvent(eventName);
    }

    public static void AnalyticsCustomEvent(string eventName, IDictionary<string, object> eventData)
    {
        Logger.LogConsole(eventName);
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
