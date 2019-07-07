using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.IO;

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
    public event Action onSavingCompleted;

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

        CoroutineManager.GetInstance().StartCoroutine(
            SaveLoadManager.SaveGameCoroutine(saveFilePath, saveables.ToArray(), CallOnSavingSucceededEvent, CallOnSavingFailedEvent, CallOnSavingCompletedEvent)
            );
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

    private void CallOnSavingCompletedEvent()
    {
        if (onSavingCompleted != null)
        {
            onSavingCompleted();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("GUARDANDO");
            SaveGame();
        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            //Loading
        }
    }

    /*public void LoadGameAsync(bool fromSaveGame, Action onSuccess, Action onFail, Action onCompletation = null)
    {
        if(fromSaveGame)
        {

        }
        else
        {

        }
    }

    private IEnumerator LoadFromSaveGameCoroutine()
    {

    }

    private IEnumerator LoadNewGameCoroutine(Action onSuccess, Action onFail, Action onCompletation = null)
    {
        AsyncOperation baseLevelUpload = SceneManager.LoadSceneAsync("BaseLevelScene");

        while(baseLevelUpload.isDone == false)
        {
            yield return null;
        }

        AsyncOperation masterSceneUpload = SceneManager.LoadSceneAsync("MasterSceneLevel1");

        while(masterSceneUpload.isDone == false)
        {
            yield return null;
        }

        AsyncOperation entitiesUpload = SceneManager.LoadSceneAsync("Entities_FirstGame");

        while(entitiesUpload.isDone == false)
        {
            yield return null;
        }

        if(onSuccess != null)
        {
            onSuccess();
        }

        if(onCompletation != null)
        {
            onCompletation();
        }

    }*/

}
