using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadingSceneManager
{
    private static LoadingSceneManager instance;

    public static LoadingSceneManager GetInstance()
    {
        if(instance == null)
        {
            instance = new LoadingSceneManager();
        }

        return instance;
    }

    private int loadingSceneIndex = 1;

    private LoadingSceneCrossFade crossfade;

    private Action onHiddenDelegate;

    private UnityAction<Scene, LoadSceneMode> onSceneLoadedDelegate;

    private LoadingSceneManager()
    {
        onHiddenDelegate = OnHidden;
        onSceneLoadedDelegate = OnSceneLoaded;
    }

    public void ShowLoadingScreen()
    {
        SceneManager.sceneLoaded += onSceneLoadedDelegate;

        SceneManager.LoadScene(loadingSceneIndex, LoadSceneMode.Additive);
    }

    public void HideLoadingScreen(Action onHidden)
    {
        if(crossfade != null)
        {
            crossfade.HideCrossFade(onHidden + onHiddenDelegate);
        }
    }

    public void HideLoadingScreen()
    {
        if (crossfade != null)
        {
            crossfade.HideCrossFade(onHiddenDelegate);
        }
    }

    private void OnHidden()
    {
        crossfade = null;

        SceneManager.UnloadSceneAsync(loadingSceneIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == loadingSceneIndex)
        {
            crossfade = GameObject.FindObjectOfType<LoadingSceneCrossFade>();

            crossfade.ShowCrossFade(null);

            SceneManager.sceneLoaded -= onSceneLoadedDelegate;
        }
    }
}
