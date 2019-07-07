using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            var obj = new GameObject("GameManager");

            instance = obj.AddComponent<GameManager>();

            instance.Init();
        }

        return instance;
    }

    public event Action onSavingBegan;
    public event Action onSavingFailed;
    public event Action onSavingSucceeded;

    public event Action onLoadingBegan;
    public event Action onLoadingFailed;
    public event Action onLoadingSucceeded;

    private HashSet<SJMonoBehaviourSaveable> saveables;

    private List<string> loadedScenes;

    public static string SaveFilePath { get; private set; }

    private void Init()
    {
        saveables = new HashSet<SJMonoBehaviourSaveable>();
        loadedScenes = new List<string>();
        SaveFilePath = Path.Combine(Application.persistentDataPath, "save.sj");
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

        foreach(SJMonoBehaviourSaveable saveable in saveables)
        {
            currentSaveables.Add(saveable);
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Logger.LogConsole("GUARDANDO");
            SaveGame();
        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            Logger.LogConsole("CARGANDO");
            LoadGame(SaveFilePath);
        }
    }

    public void LoadGame(string savePath)
    {
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
        if(loadedScenes.Count > 0)
        {
            yield return CoroutineManager.GetInstance().StartCoroutine(UnloadScenes());
        }

        yield return CoroutineManager.GetInstance().StartCoroutine(LoadScenes(new string[] { "MasterSceneLevel1" }));

        List<SJMonoBehaviourSaveable> currentSaveables = new List<SJMonoBehaviourSaveable>();

        for (int i = 0; i < saves.Length; i++)
        {
            GameplayObjectSave gameplayObjectSave = (GameplayObjectSave)saves[i].saveObject;

            GameObject gameplayGameObject = AssetLibrary.GetInstance().GetAsset<GameObject>(gameplayObjectSave.prefabName);

            Instantiate(gameplayGameObject);

            SJMonoBehaviourSaveable monoBehaviourSaveable = gameplayGameObject.GetComponent<SJMonoBehaviourSaveable>();

            typeof(SJMonoBehaviour).GetField("instanceGUID", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(monoBehaviourSaveable, gameplayObjectSave.instanceGUID);

            monoBehaviourSaveable.Load(gameplayObjectSave.save);

            currentSaveables.Add(monoBehaviourSaveable);

            yield return null;
        }

        for(int i = 0; i < currentSaveables.Count; i++)
        {
            GameplayObjectSave gameplayObjectSave = (GameplayObjectSave)saves[i].saveObject;

            currentSaveables[i].PostLoadCallback(gameplayObjectSave.save);

            yield return null;
        }
    }

    private IEnumerator UnloadScenes()
    {
        for(int i = 0; i < loadedScenes.Count; i++)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(loadedScenes[i]);

            while(unloadOperation.isDone == false)
            {
                yield return null;
            }
        }
    }

    public void LoadGame(string[] sceneNames)
    {
        CoroutineManager.GetInstance().StartCoroutine(LoadScenes(sceneNames, CallOnLoadingSucceededEvent));
    }

    private IEnumerator LoadScenes(string[] sceneNames, Action onCompletion = null)
    {
        SceneManager.LoadScene("BaseLevelScene");

        yield return null;

        for(int i = 0; i < sceneNames.Length; i++)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive);

            while(loadOperation.isDone == false)
            {
                yield return null;
            }

            loadedScenes.Add(sceneNames[i]);
        }

        if(onCompletion != null)
        {
            onCompletion();
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
