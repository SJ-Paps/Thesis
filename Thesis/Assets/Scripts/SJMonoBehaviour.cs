using UnityEngine;
using System;
using SJ.Updatables;

namespace SJ
{
    public abstract class SJMonoBehaviour : MonoBehaviour, IUpdatable
    {
        public static event Action<SJMonoBehaviour> OnInstantiation;
        public static event Action<SJMonoBehaviour> OnDestruction;

        [SerializeField]
        private bool enableUpdate = false;

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

        protected SJMonoBehaviour()
        {
            if (Application.IsInitialized)
            {
                OnInstantiation?.Invoke(this);
                Initialize();
            }
        }

        private void Initialize()
        {
            SJInitialize();
        }

        protected virtual void SJInitialize()
        {

        }

        private void Awake()
        {
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
            OnDestruction?.Invoke(this);

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

    }
}