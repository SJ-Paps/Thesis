using SJ.Management;
using System;
using System.Collections.Generic;
using UnityEngine;
using Application = SJ.Management.Application;

namespace SJ
{
    public abstract class SJMonoBehaviour : MonoBehaviour, ICompositeUpdateable
    {
        public static event Action<SJMonoBehaviour> OnInstantiation;
        public static event Action<SJMonoBehaviour> OnDestruction;

        [Header("SJ Monobehaviour Options")]
        [SerializeField]
        private bool enableUpdate = false;
        [SerializeField]
        private bool enableLateUpdate = false;
        [SerializeField]
        private bool enableFixedUpdate = false;

        [SerializeField]
        private bool disableAfterAwake;

        public bool EnableUpdate
        {
            get => enableUpdate;

            set
            {
                if (enableUpdate != value)
                {
                    enableUpdate = value;

                    UpdateUpdaterSubscriptions();
                }
            }
        }

        public bool EnableLateUpdate
        {
            get => enableLateUpdate;

            set
            {
                if(enableLateUpdate != value)
                {
                    enableLateUpdate = value;

                    UpdateUpdaterSubscriptions();
                }
            }
        }

        public bool EnableFixedUpdate
        {
            get => enableFixedUpdate;

            set
            {
                if(enableFixedUpdate != value)
                {
                    enableFixedUpdate = value;

                    UpdateUpdaterSubscriptions();
                }
            }
        }

        private int updateListenersCurrentIndex = 0;
        private int lateUpdateListenersCurrentIndex = 0;
        private int fixedUpdateListenersCurrentIndex = 0;

        private List<IUpdateListener> updateListeners = new List<IUpdateListener>();
        private List<ILateUpdateListener> lateUpdateListeners = new List<ILateUpdateListener>();
        private List<IFixedUpdateListener> fixedUpdateListeners = new List<IFixedUpdateListener>();

        private void Awake()
        {
            OnInstantiation?.Invoke(this);
            SJAwake();

            if (disableAfterAwake)
                gameObject.SetActive(false);
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

        private void UpdateUpdaterSubscriptions()
        {
            if (EnableUpdate && gameObject.activeSelf && enabled)
                Application.Instance.Updater().SubscribeToUpdate(this);
            else
                Application.Instance.Updater().UnsubscribeFromUpdate(this);

            if (EnableLateUpdate && gameObject.activeSelf && enabled)
                Application.Instance.Updater().SubscribeToLateUpdate(this);
            else
                Application.Instance.Updater().UnsubscribeFromLateUpdate(this);

            if (EnableFixedUpdate && gameObject.activeSelf && enabled)
                Application.Instance.Updater().SubscribeToFixedUpdate(this);
            else
                Application.Instance.Updater().UnsubscribeFromFixedUpdate(this);
        }

        private void OnEnable()
        {
            UpdateUpdaterSubscriptions();

            SJOnEnable();
        }

        protected virtual void SJOnEnable()
        {

        }

        private void OnDisable()
        {
            UpdateUpdaterSubscriptions();

            SJOnDisable();
        }

        protected virtual void SJOnDisable()
        {

        }

        private void OnDestroy()
        {
            OnDestruction?.Invoke(this);

            Application.Instance.Updater().UnsubscribeFromUpdate(this);
            Application.Instance.Updater().UnsubscribeFromLateUpdate(this);
            Application.Instance.Updater().UnsubscribeFromFixedUpdate(this);

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
                fixedUpdateListeners[fixedUpdateListenersCurrentIndex].DoFixedUpdate();
            }
        }

        protected virtual void SJFixedUpdate()
        {

        }

        public void SubscribeToUpdate(IUpdateListener listener)
        {
            updateListeners.Add(listener);
        }

        public void SubscribeToLateUpdate(ILateUpdateListener listener)
        {
            lateUpdateListeners.Add(listener);
        }

        public void SubscribeToFixedUpdate(IFixedUpdateListener listener)
        {
            fixedUpdateListeners.Add(listener);
        }

        public void UnsubscribeFromUpdate(IUpdateListener listener)
        {
            var indexOfListener = updateListeners.IndexOf(listener);

            if (updateListeners.Remove(listener))
            {
                if(indexOfListener <= updateListenersCurrentIndex && updateListenersCurrentIndex > 0)
                    updateListenersCurrentIndex--;
            }
        }

        public void UnsubscribeFromLateUpdate(ILateUpdateListener listener)
        {
            var indexOfListener = lateUpdateListeners.IndexOf(listener);

            if (lateUpdateListeners.Remove(listener))
            {
                if(indexOfListener <= lateUpdateListenersCurrentIndex && lateUpdateListenersCurrentIndex > 0)
                    lateUpdateListenersCurrentIndex--;
            }
        }

        public void UnsubscribeFromFixedUpdate(IFixedUpdateListener listener)
        {
            var indexOfListener = fixedUpdateListeners.IndexOf(listener);

            if (fixedUpdateListeners.Remove(listener))
            {
                if(indexOfListener <= fixedUpdateListenersCurrentIndex && fixedUpdateListenersCurrentIndex > 0)
                    fixedUpdateListenersCurrentIndex--;
            } 
        }
    }
}