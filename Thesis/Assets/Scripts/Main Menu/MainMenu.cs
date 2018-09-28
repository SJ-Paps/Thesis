using UnityEngine.SceneManagement;

public class MainMenu : SJMonoBehaviour {

    private static MainMenu instance;

    public static MainMenu GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<MainMenu>();

            if(instance == null)
            {
                instance = Instantiate<MainMenu>(SJResources.Instance.LoadGameObjectAndGetComponent<MainMenu>("MainMenuCanvas"));
            }
            
            instance.Init();
        }

        return instance;
    }

    private bool shown;

    private bool canHide;

    private void Init()
    {
        DontDestroyOnLoad(this);

        GameManager.GetInstance();

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
