using Paps.Unity;
using UnityEngine;

public class CoroutineManager : SJMonoBehaviour {

    private static CoroutineManager instance;

    public static CoroutineManager GetInstance()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject("CoroutineManager");

            instance = obj.AddComponent<CoroutineManager>();

            instance.Init();
        }

        return instance;
    }

    private void Init()
    {
        UnityUtil.DontDestroyOnLoad(gameObject);
    }
}
