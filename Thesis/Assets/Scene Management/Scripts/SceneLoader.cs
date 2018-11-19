using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    private UnityAction<Scene, LoadSceneMode> onSceneLoadedDelegate;

    private SceneLoader()
    {
        onSceneLoadedDelegate = OnSceneLoaded;
    }

    public void NewGame()
    {
        int firstLevel = GetNextSceneIndex(true);

        int[] defaultScenes = GetDefaultScenesFor(firstLevel);

        SceneManager.LoadScene(firstLevel);

        if(defaultScenes != null)
        {
            for(int i = 0; i < defaultScenes.Length; i++)
            {
                SceneManager.LoadScene(defaultScenes[i], LoadSceneMode.Additive);
            }
        }
    }

    public void LoadGame()
    {
        if(SaveLoadManager.GetInstance().SaveFileExists())
        {
            SceneManager.sceneLoaded += onSceneLoadedDelegate;

            SceneManager.LoadScene(GetNextSceneIndex(false));
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSaveables();

        SceneManager.sceneLoaded -= onSceneLoadedDelegate;
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

    public void LoadNextScene(int currentSceneIndex)
    {

    }

    public void LoadPreviousScene(int currentSceneIndex)
    {

    }

    private int GetNextSceneIndex(bool isNewGame)
    {
        //Esto esta de PlaceHolder

        if(isNewGame)
        {
            return 1;
        }
        else
        {
            return 1;
        }
    }

    private int[] GetDefaultScenesFor(int masterSceneIndex)
    {
        if(masterSceneIndex == 1)
        {
            return new int[1] { 2 };
        }

        return null;
    }
}
