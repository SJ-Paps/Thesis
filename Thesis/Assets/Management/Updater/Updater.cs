using System.Collections.Generic;
using UnityEngine;

namespace SJ.Management
{
    public class Updater : IUpdater
    {
        private class UpdaterInstance : MonoBehaviour
        {
            private Updater updateManager;

            public void SetUpdateManager(Updater updateManager)
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

        private UpdaterInstance objectInstance;

        public bool IsEnabled => objectInstance.gameObject.activeSelf && objectInstance.enabled;

        public Updater()
        {
            GameObject gameObject = new GameObject(nameof(UpdaterInstance));

            objectInstance = gameObject.AddComponent<UpdaterInstance>();

            gameObject.DontDestroyOnLoad();

            objectInstance.SetUpdateManager(this);
        }

        public void SubscribeToUpdate(IUpdateListener listener)
        {
            updateListeners.Add(listener);
        }

        public void UnsubscribeFromUpdate(IUpdateListener listener)
        {
            int indexOfListener = updateListeners.IndexOf(listener);

            if (updateListeners.Remove(listener))
            {
                if (indexOfListener <= updateListenersCurrentIndex && updateListenersCurrentIndex > 0)
                    updateListenersCurrentIndex--;
            }
        }

        public void SubscribeToLateUpdate(ILateUpdateListener listener)
        {
            lateUpdateListeners.Add(listener);
        }

        public void UnsubscribeFromLateUpdate(ILateUpdateListener listener)
        {
            int indexOfListener = lateUpdateListeners.IndexOf(listener);

            if (lateUpdateListeners.Remove(listener))
            {
                if (indexOfListener <= lateUpdateListenersCurrentIndex && lateUpdateListenersCurrentIndex > 0)
                    lateUpdateListenersCurrentIndex--;
            }
        }

        public void SubscribeToFixedUpdate(IFixedUpdateListener listener)
        {
            fixedUpdateListeners.Add(listener);
        }

        public void UnsubscribeFromFixedUpdate(IFixedUpdateListener listener)
        {
            int indexOfListener = fixedUpdateListeners.IndexOf(listener);

            if (fixedUpdateListeners.Remove(listener))
            {
                if (indexOfListener <= fixedUpdateListenersCurrentIndex && fixedUpdateListenersCurrentIndex > 0)
                    fixedUpdateListenersCurrentIndex--;
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
            for (updateListenersCurrentIndex = 0; updateListenersCurrentIndex < updateListeners.Count && IsEnabled; updateListenersCurrentIndex++)
            {
                var listenerItem = updateListeners[updateListenersCurrentIndex];

                listenerItem.DoUpdate();
            }

            updateListenersCurrentIndex = 0;
        }

        private void ExecuteLateUpdates()
        {
            for (lateUpdateListenersCurrentIndex = 0; lateUpdateListenersCurrentIndex < lateUpdateListeners.Count && IsEnabled; lateUpdateListenersCurrentIndex++)
            {
                var listenerItem = lateUpdateListeners[lateUpdateListenersCurrentIndex];

                listenerItem.DoLateUpdate();
            }

            lateUpdateListenersCurrentIndex = 0;
        }

        private void ExecuteFixedUpdates()
        {
            for (fixedUpdateListenersCurrentIndex = 0; fixedUpdateListenersCurrentIndex < fixedUpdateListeners.Count && IsEnabled; fixedUpdateListenersCurrentIndex++)
            {
                var listenerItem = fixedUpdateListeners[fixedUpdateListenersCurrentIndex];

                listenerItem.DoFixedUpdate();
            }

            fixedUpdateListenersCurrentIndex = 0;
        }
    }
}