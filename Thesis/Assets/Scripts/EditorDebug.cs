
using UnityEngine;

public static class EditorDebug
{
    public static void Log(object obj)
    {
#if UNITY_EDITOR
        Debug.Log(obj);
#endif
    }

    public static void LogWarning(object obj)
    {
#if UNITY_EDITOR
        Debug.LogWarning(obj);
#endif
    }
}
