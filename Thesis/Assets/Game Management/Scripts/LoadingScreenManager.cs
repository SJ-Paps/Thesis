using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using Paps.Unity;

public static class LoadingScreenManager
{
    public static bool IsLoadingSceneLoaded { get; private set; }

    public static AsyncOperation Open()
    {
        return SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
    }

    public static AsyncOperation Close()
    {
        return SceneManager.UnloadSceneAsync("Loading");
    }
}
