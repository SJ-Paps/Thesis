using UnityEngine;

public class EditorDebug
{
    public static void Log(object obj)
    {
#if UNITY_EDITOR && LOG
        Debug.Log(obj);
#endif
    }

    public static void LogWarning(object obj)
    {
#if UNITY_EDITOR && LOG
        Debug.LogWarning(obj);
#endif
    }

    public static void DrawLine(Vector3 origin, Vector3 end, Color color)
    {
#if UNITY_EDITOR && LOG
        Debug.DrawLine(origin, end, color);
#endif
    }
}
