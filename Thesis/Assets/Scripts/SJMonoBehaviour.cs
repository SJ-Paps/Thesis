using UnityEngine;
using System;

public abstract class SJMonoBehaviour : MonoBehaviour
{
    public string InstanceGUID { get; private set; }

    protected virtual void Awake()
    {
        InstanceGUID = GameManager.GetInstance().GetNewInstanceGUID(this);
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

    }

#endif
}
