﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using System;

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

    private Action onSceneLoadedDelegate;
    private IEnumerator waitLoadSceneAsyncCoroutine, waitNewGameLoadSceneAsyncCoroutine;

    private int loadingSceneIndex = 1, baseLevelSceneIndex = 2;

    private LoadingSceneCrossFade loadingSceneManager;

    private SceneLoader()
    {
        onSceneLoadedDelegate = OnSceneLoaded;
    }

    public void NewGame()
    {
        int firstLevel = GetCorrespondingSceneIndex(true);

        int[] defaultScenes = GetDefaultScenesFor(firstLevel);

        SceneManager.LoadScene(baseLevelSceneIndex);
        LoadingSceneManager.GetInstance().ShowLoadingScreen();

        LoadNewGameAsync(firstLevel, defaultScenes);
    }

    public void LoadGame()
    {
        if (SaveLoadManager.GetInstance().SaveFileExists())
        {
            SceneManager.LoadScene(baseLevelSceneIndex);
            LoadingSceneManager.GetInstance().ShowLoadingScreen();

            LoadSavedGameAsync(GetCorrespondingSceneIndex(false));
        }
    }

    private void LoadNewGameAsync(int masterSceneIndex, int[] defaultScenes)
    {
        waitNewGameLoadSceneAsyncCoroutine = WaitLoadSceneAsync(masterSceneIndex, defaultScenes, LoadingSceneManager.GetInstance().HideLoadingScreen);

        CoroutineManager.GetInstance().StartCoroutine(waitNewGameLoadSceneAsyncCoroutine);
    }

    private void LoadSavedGameAsync(int index)
    {
        waitLoadSceneAsyncCoroutine = WaitLoadSceneAsync(index, null, onSceneLoadedDelegate);

        CoroutineManager.GetInstance().StartCoroutine(waitLoadSceneAsyncCoroutine);
    }

    private IEnumerator WaitLoadSceneAsync(int masterSceneIndex, int[] defaultScenes, Action callback)
    {
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

        if(callback != null)
        {
            callback();
        }
    }

    private void OnSceneLoaded()
    {
        LoadSaveables();

        LoadingSceneManager.GetInstance().HideLoadingScreen();
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

                SJMonoBehaviourSaveable saveable = SJResources.Instance.LoadGameObjectAndGetComponent<SJMonoBehaviourSaveable>(current.PrefabName);

                saveables[i] = GameObject.Instantiate<SJMonoBehaviourSaveable>(saveable);

                saveables[i].Load(current);
            }

            for (int i = 0; i < saveables.Length; i++)
            {
                saveables[i].PostLoadCallback();
            }
        }
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
