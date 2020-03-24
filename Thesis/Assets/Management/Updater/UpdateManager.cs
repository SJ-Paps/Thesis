using System.Collections.Generic;
using Paps.Unity;
using UnityEngine;

namespace SJ.Updatables
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

        private List<IUpdatable> updateListeners = new List<IUpdatable>();
        private List<IUpdatable> lateUpdateListeners = new List<IUpdatable>();
        private List<IUpdatable> fixedUpdateListeners = new List<IUpdatable>();

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

        public void SubscribeToUpdate(IUpdatable updateable)
        {
            updateListeners.Add(updateable);
        }

        public void UnsubscribeFromUpdate(IUpdatable updateable)
        {
            if (updateListeners.Remove(updateable))
            {
                if (updateListenersCurrentIndex > 0)
                {
                    updateListenersCurrentIndex--;
                }
            }
        }

        public void SubscribeToLateUpdate(IUpdatable updateable)
        {
            lateUpdateListeners.Add(updateable);
        }

        public void UnsubscribeFromLateUpdate(IUpdatable updateable)
        {
            if (lateUpdateListeners.Remove(updateable))
            {
                if (lateUpdateListenersCurrentIndex > 0)
                {
                    lateUpdateListenersCurrentIndex--;
                }
            }
        }

        public void SubscribeToFixedUpdate(IUpdatable updateable)
        {
            fixedUpdateListeners.Add(updateable);
        }

        public void UnsubscribeFromFixedUpdate(IUpdatable updateable)
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

                IUpdatable updateableItem = updateListeners[updateListenersCurrentIndex];

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

                IUpdatable updateableItem = lateUpdateListeners[lateUpdateListenersCurrentIndex];

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

                IUpdatable updateableItem = fixedUpdateListeners[fixedUpdateListenersCurrentIndex];

                updateableItem.DoFixedUpdate();
            }

            fixedUpdateListenersCurrentIndex = 0;
        }
    }
}