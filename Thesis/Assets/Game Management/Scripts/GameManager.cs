using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private static string saveFilePath;

    private void Init()
    {
        saveables = new HashSet<SJMonoBehaviourSaveable>();
        saveFilePath = Path.Combine(Application.persistentDataPath, "save.sj");
    }

    public void SubscribeForSave(SJMonoBehaviourSaveable saveable)
    {
        saveables.Add(saveable);
    }

    public void DesubscribeForSave(SJMonoBehaviourSaveable saveable)
    {
        saveables.Remove(saveable);
    }

    public string GetNewInstanceGUID(SJMonoBehaviour obj)
    {
        return Guid.NewGuid().ToString();
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

        Task serializationTask = SaveLoadTool.SerializeAsync(saveFilePath, saves.ToArray());

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
            LoadGame(saveFilePath);
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

            Logger.LogConsole(((Tribal.TribalSaveData)((SaveTemplate)saves[0].saveObject).save).unNumero);

            yield return CoroutineManager.GetInstance().StartCoroutine(PrepareScene(saves));

            CallOnLoadingSucceededEvent();
        }

        deserializationTask.Dispose();
    }

    private IEnumerator PrepareScene(SaveData[] saves)
    {
        yield return null;
    }

    public void LoadGame(string[] sceneNames)
    {
        CoroutineManager.GetInstance().StartCoroutine(LoadNewGameCoroutine(sceneNames, CallOnLoadingSucceededEvent));
    }

    private IEnumerator LoadNewGameCoroutine(string[] sceneNames, Action onCompletion)
    {
        for(int i = 0; i < sceneNames.Length; i++)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneNames[i]);

            while(loadOperation.isDone == false)
            {
                yield return null;
            }
        }

        onCompletion();

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
