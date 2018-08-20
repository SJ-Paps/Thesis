#define OFF

using UnityEngine;

public class EditorDebug
{
    public static void Log(object obj)
    {
#if UNITY_EDITOR && !OFF
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
