using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    private static MainMenu instance;

    public static MainMenu GetInstance()
    {
        if(!init)
        {
            init = true;
            instance = Instantiate<MainMenu>(SJResources.Instance.LoadGameObjectAndGetComponent<MainMenu>("MainMenuCanvas"));
            instance.Init();
        }

        return instance;
    }

    private static bool init;

    private bool shown;

    private bool canHide;

    private void Init()
    {
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Show(bool shouldShow)
    {
        if (shouldShow && !shown)
        {
            Show();
        }
        else if(!shouldShow && shown)
        {
            Hide();
        }
    }

    private void Show()
    {
        shown = true;

        gameObject.SetActive(true);
    }

    private void Hide()
    {
        if(canHide)
        {
            shown = false;

            gameObject.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            canHide = false;

            Show(true);
        }
        else
        {
            canHide = true;

            Show(false);
        }
    }
}
