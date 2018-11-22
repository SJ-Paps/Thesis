using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SJUtil
{
    public static T FindActivable<T>(Vector2 center, Vector2 size, float angle) where T : IActivable
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, 1 << Reg.activableObject);

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

    public static void FindActivables<T>(Vector2 center, Vector2 size, float angle, List<T> activablesStorage) where T : IActivable
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, 1 << Reg.activableObject);

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
