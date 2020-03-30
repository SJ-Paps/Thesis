﻿using System.Collections.Generic;
using UnityEngine;
using SJ.Tools;
using SJ.Management;

public static class SJUtil
{
    public static T FindActivable<T, TActivator>(Vector2 center, Vector2 size, float angle) where T : class where TActivator : class
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, Reg.activableLayerMask);

        for (int i = 0; i < nearObjects.Length; i++)
        {
            IActivable[] activables = nearObjects[i].transform.root.GetComponentsInChildren<IActivable>();

            for(int j = 0; j < activables.Length; j++)
            {
                T activable = activables[j] as T;
                
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

    public static T FindActivable<T>(Vector2 center, Vector2 size, float angle) where T : class
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, Reg.activableLayerMask);

        for (int i = 0; i < nearObjects.Length; i++)
        {
            IActivable[] activables = nearObjects[i].transform.root.GetComponentsInChildren<IActivable>();

            for(int j = 0; j < activables.Length; j++)
            {
                T activable = activables[j] as T;

                if(activable != null)
                {
                    return activable;
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

    public static void FindActivables(Vector2 center, Vector2 size, float angle, List<IActivable> activablesStorage)
    {
        Collider2D[] nearObjects = Physics2D.OverlapBoxAll(center, size, angle, Reg.activableLayerMask);

        for (int i = 0; i < nearObjects.Length; i++)
        {
            activablesStorage.AddRange(nearObjects[i].transform.root.GetComponentsInChildren<IActivable>());
        }
    }

    public static T FindGameEntityByEntityGUIDIncludingInactive<T>(string guid) where T : class
    {
        IGameEntity saveable = FindGameEntityByEntityGUIDIncludingInactive(guid);

        if (saveable != null && saveable is T)
        {
            return saveable as T;
        }

        return null;
    }

    public static IGameEntity FindGameEntityByEntityGUIDIncludingInactive(string guid)
    {
        IGameEntity[] allSaveables = UnityUtil.FindObjectsOfTypeIncludingInactive<IGameEntity>();

        for (int i = 0; i < allSaveables.Length; i++)
        {
            IGameEntity current = allSaveables[i];

            if (current.EntityGUID == guid)
            {
                return current;
            }
        }

        return null;
    }

    public static T FindGameEntityByEntityGUID<T>(string guid) where T : class
    {
        IGameEntity saveable = FindGameEntityByEntityGUID(guid);

        if (saveable != null && saveable is T)
        {
            return saveable as T;
        }

        return null;
    }

    public static IGameEntity FindGameEntityByEntityGUID(string guid)
    {
        IGameEntity[] allSaveables = UnityUtil.FindObjectsOfType<IGameEntity>();

        for (int i = 0; i < allSaveables.Length; i++)
        {
            IGameEntity current = allSaveables[i];

            if (current.EntityGUID == guid)
            {
                return current;
            }
        }

        return null;
    }
}