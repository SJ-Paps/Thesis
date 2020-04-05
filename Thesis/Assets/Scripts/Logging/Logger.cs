using UnityEngine;

public static class Logger
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
}
