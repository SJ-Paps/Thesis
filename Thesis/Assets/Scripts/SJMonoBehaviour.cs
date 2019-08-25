using UnityEngine;
using System;

public abstract class SJMonoBehaviour : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private string instanceGUID;

    public string InstanceGUID
    {
        get
        {
            return instanceGUID;
        }

        private set
        {
            instanceGUID = value;
        }
    }

    protected virtual void Awake()
    {
        if(string.IsNullOrEmpty(InstanceGUID))
        {
            InstanceGUID = Guid.NewGuid().ToString();
        }
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void OnDestroy()
    {

    }

#if UNITY_EDITOR

    protected virtual void OnValidate()
    {
        InstanceGUID = Guid.NewGuid().ToString();
    }

#endif

    
}
