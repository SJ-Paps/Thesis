using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class SJMonoBehaviour : MonoBehaviour
{
    [SerializeField]
    public string prefabName;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        
    }

    protected virtual void OnDestroy()
    {

    }

#if UNITY_EDITOR

    private void OnValidate()
    {

    }

#endif
}
