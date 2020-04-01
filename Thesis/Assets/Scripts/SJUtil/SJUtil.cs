using System.Collections.Generic;
using UnityEngine;
using SJ.Tools;
using SJ.Management;

public static class SJUtil
{
    public static T FindGameEntityByEntityGUIDIncludingInactive<T>(string guid) where T : class
    {
        IGameEntity saveable = FindGameEntityByEntityGUIDIncludingInactive(guid);

        if(saveable != null && saveable is T)
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
