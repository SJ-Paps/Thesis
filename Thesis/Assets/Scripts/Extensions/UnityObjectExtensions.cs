using UnityEngine;

public static class UnityObjectExtensions
{
    public static T Instantiate<T>(this T obj) where T : Object
    {
        return Object.Instantiate(obj);
    }

    public static T Instantiate<T>(this T obj, Transform parent) where T : Object
    {
        return Object.Instantiate(obj, parent);
    }
}
