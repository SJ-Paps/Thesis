using UnityEngine;
using System;
using SJ.Updatables;
using System.Collections.Generic;

namespace SJ
{
    public abstract class SJMonoBehaviour : MonoBehaviour, ICompositeUpdatable
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

        private int updateListenersCurrentIndex = 0;
        private int lateUpdateListenersCurrentIndex = 0;
        private int fixedUpdateListenersCurrentIndex = 0;

        private List<IUpdatable> updateListeners = new List<IUpdatable>();
        private List<IUpdatable> lateUpdateListeners = new List<IUpdatable>();
        private List<IUpdatable> fixedUpdateListeners = new List<IUpdatable>();

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

            Application.Updater.Unsubscribe(this);

            SJOnDestroy();
        }

        protected virtual void SJOnDestroy()
        {

        }

        public void DoUpdate()
        {
            SJUpdate();

            for(updateListenersCurrentIndex = 0; updateListenersCurrentIndex < updateListeners.Count; updateListenersCurrentIndex++)
            {
                updateListeners[updateListenersCurrentIndex].DoUpdate();
            }
        }

        protected virtual void SJUpdate()
        {

        }

        public void DoLateUpdate()
        {
            SJLateUpdate();

            for (lateUpdateListenersCurrentIndex = 0; lateUpdateListenersCurrentIndex < lateUpdateListeners.Count; lateUpdateListenersCurrentIndex++)
            {
                lateUpdateListeners[lateUpdateListenersCurrentIndex].DoLateUpdate();
            }
        }

        protected virtual void SJLateUpdate()
        {
            
        }

        public void DoFixedUpdate()
        {
            SJFixedUpdate();

            for(fixedUpdateListenersCurrentIndex = 0; fixedUpdateListenersCurrentIndex < fixedUpdateListeners.Count; fixedUpdateListenersCurrentIndex++)
            {
                lateUpdateListeners[fixedUpdateListenersCurrentIndex].DoFixedUpdate();
            }
        }

        protected virtual void SJFixedUpdate()
        {

        }

        public void SubscribeToUpdate(IUpdatable updatable)
        {
            updateListeners.Add(updatable);
        }

        public void SubscribeToLateUpdate(IUpdatable updatable)
        {
            lateUpdateListeners.Add(updatable);
        }

        public void SubscribeToFixedUpdate(IUpdatable updatable)
        {
            fixedUpdateListeners.Add(updatable);
        }

        public void UnsubscribeFromUpdate(IUpdatable updatable)
        {
            if (updateListeners.Remove(updatable))
                updateListenersCurrentIndex--;
        }

        public void UnsubscribeFromLateUpdate(IUpdatable updatable)
        {
            if (lateUpdateListeners.Remove(updatable))
                lateUpdateListenersCurrentIndex--;
        }

        public void UnsubscribeFromFixedUpdate(IUpdatable updatable)
        {
            if (fixedUpdateListeners.Remove(updatable))
                fixedUpdateListenersCurrentIndex--;
        }
    }
}