using NaughtyAttributes;
using SJ.Management;
using System;
using UnityEngine;

namespace SJ.GameEntities
{
    public class GameEntity : SJMonoBehaviour, IGameEntity
    {
        [SerializeField]
        [ReadOnly]
        private string entityGUID;

        public string EntityGUID
        {
            get
            {
                return entityGUID;
            }

            protected set
            {
                entityGUID = value;
            }
        }

        protected override void SJAwake()
        {
            if (string.IsNullOrEmpty(EntityGUID))
            {
                EntityGUID = Guid.NewGuid().ToString();
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if(string.IsNullOrEmpty(EntityGUID))
                EntityGUID = Guid.NewGuid().ToString();
        }

#endif
    }
}