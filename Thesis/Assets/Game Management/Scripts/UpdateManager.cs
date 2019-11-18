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

        private List<IUpdatable> updateables;

        private UpdateManagerObject objectInstance;

        private int currentIndex = 0;

        public bool IsActive
        {
            get
            {
                return objectInstance.gameObject.activeSelf && objectInstance.enabled;
            }
        }

        public UpdateManager()
        {
            updateables = new List<IUpdatable>();

            GameObject gameObject = new GameObject(nameof(UpdateManagerObject));

            objectInstance = gameObject.AddComponent<UpdateManagerObject>();

            UnityUtil.DontDestroyOnLoad(objectInstance);

            objectInstance.SetUpdateManager(this);
        }

        public void Subscribe(IUpdatable updateable)
        {
            updateables.Add(updateable);
        }

        public void Unsubscribe(IUpdatable updateable)
        {
            if (updateables.Remove(updateable))
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
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
            for (currentIndex = 0; currentIndex < updateables.Count; currentIndex++)
            {
                if (IsActive == false)
                {
                    break;
                }

                IUpdatable updateableItem = updateables[currentIndex];

                updateableItem.DoUpdate();
            }

            currentIndex = 0;
        }

        private void ExecuteLateUpdates()
        {
            for (currentIndex = 0; currentIndex < updateables.Count; currentIndex++)
            {
                if (IsActive == false)
                {
                    break;
                }

                IUpdatable updateableItem = updateables[currentIndex];

                updateableItem.DoLateUpdate();
            }

            currentIndex = 0;
        }

        private void ExecuteFixedUpdates()
        {
            for (currentIndex = 0; currentIndex < updateables.Count; currentIndex++)
            {
                if (IsActive == false)
                {
                    break;
                }

                IUpdatable updateableItem = updateables[currentIndex];

                updateableItem.DoFixedUpdate();
            }

            currentIndex = 0;
        }
    }
}




