using UnityEngine;

public static class GameObjectExtensions
{
    public static bool ParentOrChildContainsComponent<T>(this GameObject gameObject)
    {
        return gameObject.GetComponentInChildren<T>(true) != null;
    }
}
