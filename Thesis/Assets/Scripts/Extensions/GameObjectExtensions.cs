using SJ.Tools;
using UnityEngine;

public static class GameObjectExtensions
{
    public static bool ParentOrChildContainsComponent<T>(this GameObject gameObject)
    {
        return gameObject.GetComponentInChildren<T>(true) != null;
    }

    public static void DontDestroyOnLoad(this GameObject gameObject)
    {
        UnityUtil.DontDestroyOnLoad(gameObject);
    }

    public static void Destroy(this GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }

    public static void DestroyFromDontDestroyOnLoad(this GameObject gameObject)
    {
        UnityUtil.DestroyDontDestroyOnLoadObject(gameObject);
    }

    public static void Deactivate(this GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public static void Activate(this GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public static void SwitchActiveState(this GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
