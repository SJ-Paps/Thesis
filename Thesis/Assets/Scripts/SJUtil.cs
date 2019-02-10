using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SJUtil
{
    public static T FindActivable<T, TActivator>(Vector2 center, Vector2 size, float angle) where T : IActivable<TActivator> where TActivator : class
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, Reg.activableLayerMask);

        for (int i = 0; i < nearObjects.Length; i++)
        {
            T activable = nearObjects[i].GetComponent<T>();

            if (activable != null)
            {
                return activable;
            }
        }

        return default(T);
    }

    public static void FindActivables<T, TActivator>(Vector2 center, Vector2 size, float angle, List<T> activablesStorage) where T : IActivable<TActivator> where TActivator : class
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, Reg.activableLayerMask);

        for (int i = 0; i < nearObjects.Length; i++)
        {
            T activable = nearObjects[i].GetComponent<T>();

            if (activable != null)
            {
                activablesStorage.Add(activable);
            }
        }
    }
}
