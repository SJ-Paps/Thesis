using System;

public abstract class SJMonoBehaviourSaveable : SJMonoBehaviour, ISaveable
{
    public Guid GUID { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SaveLoadManager.GetInstance().Subscribe(this);
    }

    public SaveData Save()
    {
        SaveData save = new SaveData(GUID, prefabName);

        OnSave(save);

        return save;
    }

    protected virtual void OnSave(SaveData data)
    {

    }

    public void Load(SaveData data)
    {
        if (data == null)
        {
            GUID = new Guid();
        }
        else
        {
            GUID = data.GUID;
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
}
