using UnityEngine;

public class SJMonoBehaviour : MonoBehaviour, ISaveable
{
    public string ClassName { get; private set; }

    public bool IsInstantiated
    {
        get
        {
            return gameObject.scene.name != null;
        }
    }

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
