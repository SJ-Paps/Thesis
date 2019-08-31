using Paps.Unity;
using UnityEngine;
using System.Collections;

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

    public new void StopCoroutine(IEnumerator coroutine)
    {
        try
        {
            base.StopCoroutine(coroutine);
        }
        catch { }
    }
}
