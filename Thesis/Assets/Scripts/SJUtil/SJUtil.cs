using SJ.GameEntities;
using SJ.Management;
using UnityEngine;

namespace SJ.Tools
{
    public static class SJUtil
    {
        public static IMovableObject FindMovableObject(Vector2 center, Vector2 size, float angle)
        {
            Collider2D movableCollider = Physics2D.OverlapBox(center, size, angle, Layers.MovableObject);

            if(movableCollider != null)
            {
                IMovableObject movableObject = movableCollider.transform.root.GetComponentInChildren<IMovableObject>();

                return movableObject;
            }

            return null;
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
}