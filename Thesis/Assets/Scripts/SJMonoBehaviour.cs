using UnityEngine;

public class SJMonoBehaviour : MonoBehaviour, ISaveable
{
    public string ClassName { get; private set; }

    protected virtual void Awake()
    {
        ClassName = GetType().Name;
    }

    public virtual SaveData Save()
    {
        return null;
    }

    public virtual void Load(SaveData data)
    {

    }

    public virtual void PostSaveCallback()
    {

    }

    public virtual void PostLoadCallback()
    {

    }
}
