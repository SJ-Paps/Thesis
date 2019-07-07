using UnityEngine;

public abstract class SJMonoBehaviourSaveable : SJMonoBehaviour, ISaveable
{
    [SerializeField]
    [ReadOnly]
    private string prefabName;

    public string PrefabName { get { return prefabName; } }

    protected override void Awake()
    {
        base.Awake();

        GameManager.GetInstance().SubscribeForSave(this);
    }

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

    public object Save()
    {
        return new SaveTemplate(this, GetSaveData());
    }

    public void Load(object data)
    {
        SaveTemplate saveTemplate = (SaveTemplate)data;

        LoadSaveData(saveTemplate.save);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(Application.isEditor == false)
        {
            GameManager.GetInstance().DesubscribeForSave(this);
        }
    }

    protected abstract object GetSaveData();
    protected abstract void LoadSaveData(object data);

    public abstract void PostSaveCallback();
    
    public abstract void PostLoadCallback(object dataSave);

#endif
}
