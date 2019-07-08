using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

public class GameManager {

    public class GameManagerSaveData
    {
        public int[] loadedScenesIndexes;
    }

    private static GameManager instance;

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }

        return instance;
    }

    private const string ignoreScenesOnSavingSubfix = "_igld";
    private const string gameManagerSaveDataIdentifier = "GMDATA";
    private const string baseSceneName = "Base";

    public event Action onSavingBegan;
    public event Action onSavingFailed;
    public event Action onSavingSucceeded;

    public event Action onLoadingBegan;
    public event Action onLoadingFailed;
    public event Action onLoadingSucceeded;

    public event Action onQuitting;

    private HashSet<SJMonoBehaviourSaveable> saveables;

    private string quitReturnScene;

    public bool IsInGame { get; private set; }

    public static string SaveFilePath { get; private set; }

    private GameManager()
    {
        saveables = new HashSet<SJMonoBehaviourSaveable>();
        SaveFilePath = Path.Combine(Application.persistentDataPath, "save.sj");

        onLoadingSucceeded += SetIsInGameAfterLoading;
        onQuitting += SetIsNotInGameOnQuitting;
    }

    public void SubscribeForSave(SJMonoBehaviourSaveable saveable)
    {
        saveables.Add(saveable);
    }

    public void DesubscribeForSave(SJMonoBehaviourSaveable saveable)
    {
        saveables.Remove(saveable);
    }

    public void SaveGame()
    {
        CallOnSavingBeganEvent();

        CoroutineManager.GetInstance().StartCoroutine(GetAllSavesAndAWaitSerializationCoroutine());
    }

    private IEnumerator GetAllSavesAndAWaitSerializationCoroutine()
    {
        List<SJMonoBehaviourSaveable> currentSaveables = new List<SJMonoBehaviourSaveable>();
        List<SaveData> saves = new List<SaveData>();

        currentSaveables.AddRange(saveables);

        saves.Add(
            new SaveData(gameManagerSaveDataIdentifier, 
            new GameManagerSaveData()
            {
                loadedScenesIndexes = GetSaveableScenes()
            }
            ));

        for(int i = 0; i < currentSaveables.Count; i++)
        {
            SJMonoBehaviourSaveable saveable = currentSaveables[i];
            saves.Add(new SaveData(saveable.InstanceGUID, saveable.Save()));
            yield return null;
        }

        for(int i = 0; i < currentSaveables.Count; i++)
        {
            if (saveables.Contains(currentSaveables[i]) == false)
            {
                currentSaveables.RemoveAt(i);
                saves.RemoveAt(i);
                i--;
            }
        }

        for(int i = 0; i < currentSaveables.Count; i++)
        {
            currentSaveables[i].PostSaveCallback();
        }

        Task serializationTask = SaveLoadTool.SerializeAsync(SaveFilePath, saves.ToArray());

        while(serializationTask.IsCompleted == false)
        {
            yield return null;
        }

        if (serializationTask.IsFaulted)
        {
            CallOnSavingFailedEvent();
        }
        else
        {
            CallOnSavingSucceededEvent();
        }

        serializationTask.Dispose();
    }

    public void SetOnQuitReturnScene(string sceneName)
    {
        quitReturnScene = sceneName;
    }

    public void LoadGame(string savePath)
    {
        CallOnLoadingBeganEvent();

        CoroutineManager.GetInstance().StartCoroutine(LoadFromSaveGameCoroutine(savePath));
    }

    private IEnumerator LoadFromSaveGameCoroutine(string savePath)
    {
        Task<SaveData[]> deserializationTask = SaveLoadTool.DeserializeAsync(savePath);

        while(deserializationTask.IsCompleted == false)
        {
            yield return null;
        }

        if(deserializationTask.IsFaulted)
        {
            CallOnLoadingFailedEvent();
        }
        else
        {
            SaveData[] saves = deserializationTask.Result;

            yield return CoroutineManager.GetInstance().StartCoroutine(PrepareScene(saves));

            CallOnLoadingSucceededEvent();
        }

        deserializationTask.Dispose();
    }

    private IEnumerator PrepareScene(SaveData[] saves)
    {
        yield return CoroutineManager.GetInstance().StartCoroutine(LoadGameplayScenes(GetSaveDataFrom(saves).loadedScenesIndexes));

        List<SJMonoBehaviourSaveable> currentSaveables = new List<SJMonoBehaviourSaveable>();
        List<GameplayObjectSave> gameplayObjectSaves = new List<GameplayObjectSave>();

        for (int i = 0; i < saves.Length; i++)
        {
            GameplayObjectSave gameplayObjectSave = saves[i].saveObject as GameplayObjectSave;

            if(gameplayObjectSave == null)
            {
                continue;
            }

            GameObject gameplayGameObject = SJResources.LoadAsset<GameObject>(gameplayObjectSave.prefabName);

            gameplayGameObject = GameObject.Instantiate(gameplayGameObject);

            SJMonoBehaviourSaveable monoBehaviourSaveable = gameplayGameObject.GetComponent<SJMonoBehaviourSaveable>();

            typeof(SJMonoBehaviour).GetField("instanceGUID", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(monoBehaviourSaveable, gameplayObjectSave.instanceGUID);
            
            monoBehaviourSaveable.Load(gameplayObjectSave.save);

            currentSaveables.Add(monoBehaviourSaveable);
            gameplayObjectSaves.Add(gameplayObjectSave);

            yield return null;
        }

        for(int i = 0; i < currentSaveables.Count; i++)
        {
            currentSaveables[i].PostLoadCallback(gameplayObjectSaves[i].save);

            yield return null;
        }
    }

    private GameManagerSaveData GetSaveDataFrom(SaveData[] saves)
    {
        for(int i = 0; i < saves.Length; i++)
        {
            if(saves[i].identifier == gameManagerSaveDataIdentifier)
            {
                return (GameManagerSaveData)saves[i].saveObject;
            }
        }

        return null;
    }

    public void LoadGame(string[] sceneNames)
    {
        CoroutineManager.GetInstance().StartCoroutine(LoadGameplayScenes(sceneNames, CallOnLoadingSucceededEvent));
    }

    private IEnumerator LoadGameplayScenes(string[] sceneNames, Action onCompletion = null)
    {
        SceneManager.LoadScene(baseSceneName, LoadSceneMode.Additive);

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseSceneName));

        yield return null;

        for(int i = 0; i < sceneNames.Length; i++)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive);

            while(loadOperation.isDone == false)
            {
                yield return null;
            }
        }

        if(onCompletion != null)
        {
            onCompletion();
        }
    }

    private IEnumerator LoadGameplayScenes(int[] sceneIndexes, Action onCompletion = null)
    {
        SceneManager.LoadScene(baseSceneName);

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseSceneName));

        yield return null;

        for (int i = 0; i < sceneIndexes.Length; i++)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndexes[i], LoadSceneMode.Additive);

            while (loadOperation.isDone == false)
            {
                yield return null;
            }
        }

        if (onCompletion != null)
        {
            onCompletion();
        }
    }

    public void QuitGame()
    {
        CallOnQuittingEvent();

        SceneManager.LoadScene(quitReturnScene);
    }

    private void SetIsInGameAfterLoading()
    {
        IsInGame = true;
    }

    private void SetIsNotInGameOnQuitting()
    {
        IsInGame = false;
    }

    private int[] GetSaveableScenes()
    {
        List<int> saveableScenes = new List<int>();

        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene current = SceneManager.GetSceneAt(i);

            if (current.name.HasSubfix(ignoreScenesOnSavingSubfix) == false || current.name == baseSceneName)
            {
                saveableScenes.Add(current.buildIndex);
            }
        }

        return saveableScenes.ToArray();
    }

    private void CallOnQuittingEvent()
    {
        if(onQuitting != null)
        {
            onQuitting();
        }
    }

    private void CallOnSavingSucceededEvent()
    {
        if (onSavingSucceeded != null)
        {
            onSavingSucceeded();
        }
    }

    private void CallOnSavingFailedEvent()
    {
        if (onSavingFailed != null)
        {
            onSavingFailed();
        }
    }

    private void CallOnSavingBeganEvent()
    {
        if (onSavingBegan != null)
        {
            onSavingBegan();
        }
    }

    private void CallOnLoadingBeganEvent()
    {
        if (onLoadingBegan != null)
        {
            onLoadingBegan();
        }
    }

    private void CallOnLoadingFailedEvent()
    {
        if (onLoadingFailed != null)
        {
            onLoadingFailed();
        }
    }

    private void CallOnLoadingSucceededEvent()
    {
        if (onLoadingSucceeded != null)
        {
            onLoadingSucceeded();
        }
    }

}
