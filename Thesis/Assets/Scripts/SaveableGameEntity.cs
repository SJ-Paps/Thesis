using NaughtyAttributes;
using UnityEngine;

namespace SJ.GameEntities
{
    public abstract class SaveableGameEntity : GameEntity, ISaveableGameEntity
    {
        [SerializeField]
        [ReadOnly]
        private string prefabName;

        public string PrefabName { get { return prefabName; } }

        protected override void SJAwake()
        {
            SJ.Application.GameManager.SubscribeSaveable(this);
        }

        public GameplayObjectSave Save()
        {
            return new GameplayObjectSave(EntityGUID, PrefabName, GetSaveData());
        }

        public void Load(GameplayObjectSave data)
        {
            EntityGUID = data.EntityGuid;
            LoadSaveData(data.Save);
        }

        public void PostSaveCallback()
        {
            OnPostSave();
        }

        public void PostLoadCallback(GameplayObjectSave data)
        {
            OnPostLoad(data.Save);
        }

        protected override void SJOnDestroy()
        {
            if (UnityEngine.Application.isEditor == false)
            {
                Application.GameManager.UnsubscribeSaveable(this);
            }
        }

        protected abstract object GetSaveData();
        protected abstract void LoadSaveData(object data);
        protected abstract void OnPostSave();
        protected abstract void OnPostLoad(object data);

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            GameObject prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource<GameObject>(gameObject);

            if (prefab != null)
            {
                prefabName = prefab.name;
            }
        }

#endif
    }
}