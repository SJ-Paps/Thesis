
using UnityEngine;

public static class EditorDebug
{
    public static void Log(object obj)
    {
#if UNITY_EDITOR
        Debug.Log(obj);
#endif
    }
}
