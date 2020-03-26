using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Application = SJ.Management.Application;

namespace SJ.GameEntities
{
    public abstract class SaveableGameEntity : GameEntity, ISaveableGameEntity
    {
        [SerializeField]
        [ReadOnly]
        private string prefabName;

        public string PrefabName { get { return prefabName; } }

        private List<ISaveProcessor> saveProcessors = new List<ISaveProcessor>();

        protected override void SJAwake()
        {
            Application.GameManager.SubscribeSaveable(this);
        }

        public GameplayObjectSave Save()
        {
            var save = GetSaveData();

            for (int i = 0; i < saveProcessors.Count; i++)
                saveProcessors[i].Save(save);

            return new GameplayObjectSave(EntityGUID, PrefabName, save);
        }

        public void Load(GameplayObjectSave data)
        {
            EntityGUID = data.EntityGuid;
            LoadSaveData(data.Save);

            for (int i = 0; i < saveProcessors.Count; i++)
                saveProcessors[i].Load(data.Save);
        }

        public void PostSaveCallback()
        {
            OnPostSave();
        }

        public void PostLoadCallback(GameplayObjectSave data)
        {
            OnPostLoad(data.Save);
        }

        public void AddSaveProcessor(ISaveProcessor saveProcessor)
        {
            if (saveProcessors.Contains(saveProcessor) == false)
                saveProcessors.Add(saveProcessor);
        }

        public bool RemoveSaveProcessor(ISaveProcessor saveProcessor)
        {
            return saveProcessors.Remove(saveProcessor);
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

        protected virtual void OnPostSave() { }
        protected virtual void OnPostLoad(object data) { }

#if UNITY_EDITOR

        protected override void SJValidate()
        {
            SavePrefabName();
        }

        public void SavePrefabName()
        {
            GameObject prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource<GameObject>(gameObject);

            if (prefab != null)
            {
                prefabName = prefab.name;
            }
        }
#endif
    }
}