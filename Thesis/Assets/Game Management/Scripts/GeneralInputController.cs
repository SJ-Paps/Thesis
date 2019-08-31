using Paps.Unity;
using UnityEngine;

public class GeneralInputController : SJMonoBehaviour
{
    private static GeneralInputController instance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        GameManager.GetInstance().onLoadingBegan += DestroyOnQuittingGameOrLoading;
        GameManager.GetInstance().onLoadingSucceeded += InstantiateInGame;
        GameManager.GetInstance().onQuitting += DestroyOnQuittingGameOrLoading;
    }

    private static void DestroyOnQuittingGameOrLoading()
    {
        if(instance != null)
        {
            UnityUtil.DestroyDontDestroyOnLoadObject(instance.gameObject);
        }
    }

    private static void InstantiateInGame()
    {
        instance = new GameObject(nameof(GeneralInputController)).AddComponent<GeneralInputController>();

        UnityUtil.DontDestroyOnLoad(instance.gameObject);
    }

    protected override void SJAwake()
    {
        MainMenu.GetInstance().Hide();
    }

    protected override void SJUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(MainMenu.GetInstance().Shown)
            {
                MainMenu.GetInstance().Hide();
            }
            else
            {
                MainMenu.GetInstance().Show();
            }
        }
    }
}
