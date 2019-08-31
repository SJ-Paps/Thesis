using System.Collections.Generic;
using Paps.Unity;
using UnityEngine;

public class UpdateManager
{
    private class UpdateManagerObjectInstance : SJMonoBehaviour
    {
        public void Update()
        {
            UpdateManager.GetInstance().ExecuteUpdates();
        }

        public void LateUpdate()
        {
            UpdateManager.GetInstance().ExecuteLateUpdates();
        }

        public void FixedUpdate()
        {
            UpdateManager.GetInstance().ExecuteFixedUpdates();
        }
    }

    private static UpdateManager instance;

    public static UpdateManager GetInstance()
    {
        if(instance == null)
        {
            instance = new UpdateManager();

            instance.Initialize();
        }

        return instance;
    }

    private List<IUpdateable> updateables;

    private UpdateManagerObjectInstance objectInstance;

    private int currentIndex = 0;

    public bool IsActive
    {
        get
        {
            return objectInstance.gameObject.activeSelf && objectInstance.enabled;
        }
    }

    private void Initialize()
    {
        updateables = new List<IUpdateable>();

        GameObject gameObject = new GameObject(nameof(UpdateManagerObjectInstance));

        objectInstance = gameObject.AddComponent<UpdateManagerObjectInstance>();

        UnityUtil.DontDestroyOnLoad(objectInstance);
    }

    public void Subscribe(IUpdateable updateable)
    {
        updateables.Add(updateable);
    }

    public void Unsubscribe(IUpdateable updateable)
    {
        if(updateables.Remove(updateable))
        {
            if(currentIndex > 0)
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
        for(currentIndex = 0; currentIndex < updateables.Count; currentIndex++)
        {
            if(IsActive == false)
            {
                break;
            }

            IUpdateable updateableItem = updateables[currentIndex];

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

            IUpdateable updateableItem = updateables[currentIndex];

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

            IUpdateable updateableItem = updateables[currentIndex];

            updateableItem.DoFixedUpdate();
        }

        currentIndex = 0;
    }
}


