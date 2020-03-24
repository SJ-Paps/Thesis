using UnityEngine;

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
}
