using UnityEngine;
using SJ.Game;

public abstract class SJMonoBehaviourSaveable : SJMonoBehaviour, ISaveable
{
    [SerializeField]
    [ReadOnly]
    private string prefabName;

    public string PrefabName { get { return prefabName; } }

    protected override void SJAwake()
    {
        GameManager.GetInstance().SubscribeForSave(this);
    }

    public object Save()
    {
        return new GameplayObjectSave(this, GetSaveData());
    }

    public void Load(object data)
    {
        LoadSaveData(data);
    }

    protected override void SJOnDestroy()
    {
        if (Application.isEditor == false)
        {
            GameManager.GetInstance().DesubscribeForSave(this);
        }
    }

    protected abstract object GetSaveData();
    protected abstract void LoadSaveData(object data);

    public abstract void PostSaveCallback();

    public abstract void PostLoadCallback(object dataSave);

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
