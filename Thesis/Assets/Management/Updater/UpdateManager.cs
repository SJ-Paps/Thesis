using System.Collections.Generic;
using SJ.Tools;
using UnityEngine;

namespace SJ.Management
{
    public class UpdateManager : IUpdater
    {
        private class UpdateManagerObject : MonoBehaviour
        {
            private UpdateManager updateManager;

            public void SetUpdateManager(UpdateManager updateManager)
            {
                this.updateManager = updateManager;
            }

            public void Update()
            {
                updateManager.ExecuteUpdates();
            }

            public void LateUpdate()
            {
                updateManager.ExecuteLateUpdates();
            }

            public void FixedUpdate()
            {
                updateManager.ExecuteFixedUpdates();
            }
        }

        private int updateListenersCurrentIndex = 0;
        private int lateUpdateListenersCurrentIndex = 0;
        private int fixedUpdateListenersCurrentIndex = 0;

        private List<IUpdateListener> updateListeners = new List<IUpdateListener>();
        private List<ILateUpdateListener> lateUpdateListeners = new List<ILateUpdateListener>();
        private List<IFixedUpdateListener> fixedUpdateListeners = new List<IFixedUpdateListener>();

        private UpdateManagerObject objectInstance;

        public bool IsActive
        {
            get
            {
                return objectInstance.gameObject.activeSelf && objectInstance.enabled;
            }
        }

        public UpdateManager()
        {
            GameObject gameObject = new GameObject(nameof(UpdateManagerObject));

            objectInstance = gameObject.AddComponent<UpdateManagerObject>();

            UnityUtil.DontDestroyOnLoad(objectInstance);

            objectInstance.SetUpdateManager(this);
        }

        public void SubscribeToUpdate(IUpdateListener updateable)
        {
            updateListeners.Add(updateable);
        }

        public void UnsubscribeFromUpdate(IUpdateListener updateable)
        {
            if (updateListeners.Remove(updateable))
            {
                if (updateListenersCurrentIndex > 0)
                {
                    updateListenersCurrentIndex--;
                }
            }
        }

        public void SubscribeToLateUpdate(ILateUpdateListener updateable)
        {
            lateUpdateListeners.Add(updateable);
        }

        public void UnsubscribeFromLateUpdate(ILateUpdateListener updateable)
        {
            if (lateUpdateListeners.Remove(updateable))
            {
                if (lateUpdateListenersCurrentIndex > 0)
                {
                    lateUpdateListenersCurrentIndex--;
                }
            }
        }

        public void SubscribeToFixedUpdate(IFixedUpdateListener updateable)
        {
            fixedUpdateListeners.Add(updateable);
        }

        public void UnsubscribeFromFixedUpdate(IFixedUpdateListener updateable)
        {
            if (fixedUpdateListeners.Remove(updateable))
            {
                if (fixedUpdateListenersCurrentIndex > 0)
                {
                    fixedUpdateListenersCurrentIndex--;
                }
            }
        }

        public void Disable()
        {
            objectInstance.gameObject.SetActive(false);
        }

        public void Enable()
        {
            objectInstance.gameObject.SetActive(true);
        }

        private void ExecuteUpdates()
        {
            for (updateListenersCurrentIndex = 0; updateListenersCurrentIndex < updateListeners.Count; updateListenersCurrentIndex++)
            {
                if (IsActive == false)
                {
                    break;
                }

                var updateableItem = updateListeners[updateListenersCurrentIndex];

                updateableItem.DoUpdate();
            }

            updateListenersCurrentIndex = 0;
        }

        private void ExecuteLateUpdates()
        {
            for (lateUpdateListenersCurrentIndex = 0; lateUpdateListenersCurrentIndex < lateUpdateListeners.Count; lateUpdateListenersCurrentIndex++)
            {
                if (IsActive == false)
                {
                    break;
                }

                var updateableItem = lateUpdateListeners[lateUpdateListenersCurrentIndex];

                updateableItem.DoLateUpdate();
            }

            lateUpdateListenersCurrentIndex = 0;
        }

        private void ExecuteFixedUpdates()
        {
            for (fixedUpdateListenersCurrentIndex = 0; fixedUpdateListenersCurrentIndex < fixedUpdateListeners.Count; fixedUpdateListenersCurrentIndex++)
            {
                if (IsActive == false)
                {
                    break;
                }

                var updateableItem = fixedUpdateListeners[fixedUpdateListenersCurrentIndex];

                updateableItem.DoFixedUpdate();
            }

            fixedUpdateListenersCurrentIndex = 0;
        }
    }
}