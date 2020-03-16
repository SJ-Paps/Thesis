using UnityEngine;
using System;
using SJ.Updatables;
using NaughtyAttributes;
using SJ.Game;

public abstract class SJMonoBehaviour : MonoBehaviour, IUpdatable, IGameplayEntity
{
    public static event Action<SJMonoBehaviour> onInstantiation;
    public static event Action<SJMonoBehaviour> onDestruction;

    [SerializeField]
    [ReadOnly]
    private string instanceGUID;

    public string InstanceGuid
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
            if (enableUpdate != value)
            {
                enableUpdate = value;

                UpdateEnableUpdateSubscription();
            }
        }
    }

    private void UpdateEnableUpdateSubscription()
    {
        if (EnableUpdate && gameObject.activeSelf && this.enabled)
        {
            SJ.Application.Updater.Subscribe(this);
        }
        else
        {
            SJ.Application.Updater.Unsubscribe(this);
        }
    }


    private void Awake()
    {
        if(onInstantiation != null)
        {
            onInstantiation(this);
        }

        if (string.IsNullOrEmpty(InstanceGuid))
        {
            InstanceGuid = Guid.NewGuid().ToString();
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
        if(onDestruction != null)
        {
            onDestruction(this);
        }

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
        InstanceGuid = Guid.NewGuid().ToString();
    }

#endif


}

