using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class SJMonoBehaviour : MonoBehaviour
{
    [SerializeField]
    public string prefabName;

    private IConfiguration cachedConfiguration;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        
    }

    protected virtual void OnDestroy()
    {

    }

    public IConfiguration GetConfiguration()
    {
        if(cachedConfiguration == null)
        {
            cachedConfiguration = GetComponent<IConfiguration>();
        }

        return cachedConfiguration;
    }

#if UNITY_EDITOR

    private void OnValidate()
    {

    }

#endif
}
