using System;
using UnityEngine;

public abstract class SJMonoBehaviourSaveable : SJMonoBehaviour, ISaveable
{
    public Guid saveGUID { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SaveLoadManager.GetInstance().Subscribe(this);
        
    }

    protected override void Start()
    {
        base.Start();

        if (saveGUID == default(Guid))
        {
            saveGUID = Guid.NewGuid();
        }
    }

    public SaveData Save()
    {
        SaveData save = new SaveData(saveGUID, prefabName);

        OnSave(save);

        return save;
    }

    protected virtual void OnSave(SaveData data)
    {

    }

    public void Load(SaveData data)
    {
        if (data != null)
        {
            saveGUID = data.GUID;
            OnLoad(data);
        }
    }

    protected virtual void OnLoad(SaveData data)
    {

    }

    public virtual void PostSaveCallback()
    {

    }

    public virtual void PostLoadCallback()
    {
        
    }

    public static T GetSJMonobehaviourSaveableBySaveGUID<T>(Guid guid) where T : class
    {
        SJMonoBehaviourSaveable saveable = GetSJMonoBehaviourSaveableBySaveGUID(guid);

        if(saveable != null && saveable is T)
        {
            return saveable as T;
        }

        return null;
    }

    public static SJMonoBehaviourSaveable GetSJMonoBehaviourSaveableBySaveGUID(Guid guid)
    {
        SJMonoBehaviourSaveable[] allSaveables = FindObjectsOfType<SJMonoBehaviourSaveable>();

        for (int i = 0; i < allSaveables.Length; i++)
        {
            SJMonoBehaviourSaveable current = allSaveables[i];

            if (current.saveGUID == guid)
            {
                return current;
            }
        }

        return null;
    }
}
