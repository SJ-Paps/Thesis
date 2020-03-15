using Paps.Unity;
using UnityEngine;

namespace SJ.Game
{
    public class GeneralInputController : SJMonoBehaviour
    {
        private static GeneralInputController instance;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Application.OnInitialized += () =>
            {
                Application.GameManager.onLoadingBegan += DestroyOnQuittingGameOrLoading;
                Application.GameManager.onLoadingSucceeded += InstantiateInGame;
                Application.GameManager.onQuitting += DestroyOnQuittingGameOrLoading;
            };
        }

        private static void DestroyOnQuittingGameOrLoading()
        {
            if (instance != null)
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (MainMenu.GetInstance().Shown)
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

}

