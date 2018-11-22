using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader
{
    private static SceneLoader instance;

    public static SceneLoader GetInstance()
    {
        if(instance == null)
        {
            instance = new SceneLoader();
        }

        return instance;
    }

    private Action onSceneLoadedFromSavedGameDelegate, onSceneLoadedFromNewGameDelegate;
    private IEnumerator waitLoadSceneAsyncCoroutine, waitNewGameLoadSceneAsyncCoroutine;

    private int baseLevelSceneIndex = 2;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(2f);

    private SceneLoader()
    {
        onSceneLoadedFromSavedGameDelegate = OnSceneLoadedFromSavedGame;
        onSceneLoadedFromNewGameDelegate = OnSceneLoadedFromNewGame;
    }

    public void NewGame()
    {
        int firstLevel = GetCorrespondingSceneIndex(true);

        int[] defaultScenes = GetDefaultScenesFor(firstLevel);
        
        /*SceneManager.LoadScene(baseLevelSceneIndex);
        ShowLoadingScreen();*/

        LoadNewGameAsync(firstLevel, defaultScenes);
    }

    public void LoadGame()
    {
        if (SaveLoadManager.GetInstance().SaveFileExists())
        {
            /*SceneManager.LoadScene(baseLevelSceneIndex);
            ShowLoadingScreen();*/

            LoadSavedGameAsync(GetCorrespondingSceneIndex(false));
        }
    }

    private void LoadNewGameAsync(int masterSceneIndex, int[] defaultScenes)
    {
        waitNewGameLoadSceneAsyncCoroutine = WaitLoadSceneAsync(masterSceneIndex, defaultScenes, onSceneLoadedFromNewGameDelegate);

        CoroutineManager.GetInstance().StartCoroutine(waitNewGameLoadSceneAsyncCoroutine);
    }

    private void LoadSavedGameAsync(int index)
    {
        waitLoadSceneAsyncCoroutine = WaitLoadSceneAsync(index, null, onSceneLoadedFromSavedGameDelegate);

        CoroutineManager.GetInstance().StartCoroutine(waitLoadSceneAsyncCoroutine);
    }

    private IEnumerator WaitLoadSceneAsync(int masterSceneIndex, int[] defaultScenes, Action callback)
    {
        SceneManager.LoadScene(baseLevelSceneIndex);

        yield return null;

        ShowLoadingScreen();

        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(masterSceneIndex, LoadSceneMode.Additive);

        while(!operation.isDone)
        {
            yield return null;
        }

        if (defaultScenes != null)
        {
            for (int i = 0; i < defaultScenes.Length; i++)
            {
                operation = SceneManager.LoadSceneAsync(defaultScenes[i], LoadSceneMode.Additive);

                while(!operation.isDone)
                {
                    yield return null;
                }
            }
        }

        yield return waitForSeconds;

        if (callback != null)
        {
            callback();
        }
    }

    private void OnSceneLoadedFromSavedGame()
    {
        LoadSaveables();

        HideLoadingScreen();
    }

    private void OnSceneLoadedFromNewGame()
    {
        HideLoadingScreen();
    }

    private void LoadSaveables()
    {
        SaveData[] saves = SaveLoadManager.GetInstance().LoadSaves();

        if (saves != null)
        {
            SJMonoBehaviourSaveable[] saveables = new SJMonoBehaviourSaveable[saves.Length];

            for (int i = 0; i < saves.Length; i++)
            {
                SaveData current = saves[i];

                Debug.Log(current.PrefabName);

                GameObject obj = SJResources.Instance.LoadAsset<GameObject>(current.PrefabName);

                Debug.Log(obj);

                saveables[i] = GameObject.Instantiate(obj).GetComponentInChildren<SJMonoBehaviourSaveable>();

                saveables[i].Load(current);
            }

            for (int i = 0; i < saveables.Length; i++)
            {
                saveables[i].PostLoadCallback();
            }
        }
    }

    private void ShowLoadingScreen()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    private void HideLoadingScreen()
    {
        SceneManager.UnloadSceneAsync(1);
    }

    private int GetCorrespondingSceneIndex(bool isNewGame)
    {
        //Esto esta de PlaceHolder

        if(isNewGame)
        {
            return 3;
        }
        else
        {
            return 3;
        }
    }

    private int[] GetDefaultScenesFor(int masterSceneIndex)
    {
        if(masterSceneIndex == 3)
        {
            return new int[1] { 4 };
        }

        return null;
    }
}
