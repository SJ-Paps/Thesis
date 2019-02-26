using System.Collections.Generic;
using UnityEngine;

public static class SJUtil
{
    public static T FindActivable<T, TActivator>(Vector2 center, Vector2 size, float angle) where T : class where TActivator : class
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, Reg.activableLayerMask);

        for (int i = 0; i < nearObjects.Length; i++)
        {
            MonoBehaviour[] monoBehaviours = nearObjects[i].transform.root.GetComponentsInChildren<MonoBehaviour>();

            for(int j = 0; j < monoBehaviours.Length; j++)
            {
                T activable = monoBehaviours[j] as T;
                
                if (activable != null)
                {
                    IActivable<TActivator> asSpecificActivable = activable as IActivable<TActivator>;

                    if(asSpecificActivable != null)
                    {
                        return activable;
                    }
                }
            }
        }

        return default;
    }

    public static void FindActivables<T, TActivator>(Vector2 center, Vector2 size, float angle, List<T> activablesStorage) where T : class where TActivator : class
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, Reg.activableLayerMask);

        for (int i = 0; i < nearObjects.Length; i++)
        {
            MonoBehaviour[] monoBehaviours = nearObjects[i].transform.root.GetComponentsInChildren<MonoBehaviour>();

            for(int j = 0; j < monoBehaviours.Length; j++)
            {
                T activable = monoBehaviours[i] as T;

                if (activable != null)
                {
                    IActivable<TActivator> asSpecificActivable = activable as IActivable<TActivator>;

                    if (asSpecificActivable != null)
                    {
                        activablesStorage.Add(activable);
                    }
                }
            }
        }
    }
}
