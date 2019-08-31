using System.Collections.Generic;
using Paps.Unity;
using UnityEngine;

public class UpdateManager
{
    private class UpdateManagerObjectInstance : SJMonoBehaviour
    {
        public void Update()
        {
            UpdateManager.GetInstance().Update();
        }

        public void LateUpdate()
        {
            UpdateManager.GetInstance().LateUpdate();
        }

        public void FixedUpdate()
        {
            UpdateManager.GetInstance().FixedUpdate();
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

    private GameObject gameObjectInstance;

    private int currentIndex = 0;

    private void Initialize()
    {
        updateables = new List<IUpdateable>();

        gameObjectInstance = new GameObject(nameof(UpdateManagerObjectInstance));

        gameObjectInstance.AddComponent<UpdateManagerObjectInstance>();

        UnityUtil.DontDestroyOnLoad(gameObjectInstance);
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
    
    void Update()
    {
        for(currentIndex = 0; currentIndex < updateables.Count; currentIndex++)
        {
            IUpdateable updateableItem = updateables[currentIndex];

            updateableItem.DoUpdate();
        }

        currentIndex = 0;
    }

    void LateUpdate()
    {
        for (currentIndex = 0; currentIndex < updateables.Count; currentIndex++)
        {
            IUpdateable updateableItem = updateables[currentIndex];

            updateableItem.DoLateUpdate();
        }

        currentIndex = 0;
    }

    void FixedUpdate()
    {
        for (currentIndex = 0; currentIndex < updateables.Count; currentIndex++)
        {
            IUpdateable updateableItem = updateables[currentIndex];

            updateableItem.DoFixedUpdate();
        }

        currentIndex = 0;
    }
}


