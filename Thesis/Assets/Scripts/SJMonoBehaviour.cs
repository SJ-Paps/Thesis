using UnityEngine;
using System;

public abstract class SJMonoBehaviour : MonoBehaviour, IUnityUpdateable
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

    [SerializeField]
    private bool enableUpdate = true;

    public bool EnableUpdate
    {
        get
        {
            return enableUpdate;
        }

        set
        {
            if(enableUpdate != value)
            {
                enableUpdate = value;

                UpdateEnableUpdateSubscription();
            }
        }
    }

    private void UpdateEnableUpdateSubscription()
    {
        if(EnableUpdate && gameObject.activeSelf && this.enabled)
        {
            UpdateManager.GetInstance().Subscribe(this);
        }
        else
        {
            UpdateManager.GetInstance().Unsubscribe(this);
        }
    }


    private void Awake()
    {
        if (string.IsNullOrEmpty(InstanceGUID))
        {
            InstanceGUID = Guid.NewGuid().ToString();
        }

        SJAwake();
    }

    protected virtual void SJAwake()
    {

    }

    private void Start()
    {
        SJStart();
    }

    protected virtual void SJStart()
    {

    }

    private void OnEnable()
    {
        UpdateEnableUpdateSubscription();

        SJOnEnable();
    }
    
    protected virtual void SJOnEnable()
    {

    }

    private void OnDisable()
    {
        UpdateEnableUpdateSubscription();

        SJOnDisable();
    }

    protected virtual void SJOnDisable()
    {

    }

    private void OnDestroy()
    {
        SJOnDestroy();
    }

    protected virtual void SJOnDestroy()
    {

    }

    public void DoUpdate()
    {
        SJUpdate();
    }

    protected virtual void SJUpdate()
    {

    }

    public void DoLateUpdate()
    {
        SJLateUpdate();
    }

    protected virtual void SJLateUpdate()
    {

    }

    public void DoFixedUpdate()
    {
        SJFixedUpdate();
    }

    protected virtual void SJFixedUpdate()
    {

    }

#if UNITY_EDITOR

    protected virtual void OnValidate()
    {
        InstanceGUID = Guid.NewGuid().ToString();
    }

#endif

    
}
