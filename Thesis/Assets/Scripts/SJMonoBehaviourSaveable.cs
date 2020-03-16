using NaughtyAttributes;
using SJ.Save;
using UnityEngine;

public abstract class SJMonoBehaviourSaveable : SJMonoBehaviour, ISaveable
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
        return new GameplayObjectSave(InstanceGuid, PrefabName, GetSaveData());
    }

    public void Load(GameplayObjectSave data)
    {
        LoadSaveData(data.save);
    }

    public void PostSaveCallback()
    {
        OnPostSave();
    }

    public void PostLoadCallback(GameplayObjectSave data)
    {
        OnPostLoad(data.save);
    }

    protected override void SJOnDestroy()
    {
        if (Application.isEditor == false)
        {
            SJ.Application.GameManager.UnsubscribeSaveable(this);
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
