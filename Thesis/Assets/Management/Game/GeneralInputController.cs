using Paps.Unity;
using SJ.UI;
using UnityEngine;
using UniRx;

namespace SJ.Game
{
    public class GeneralInputController : SJMonoBehaviour
    {
        private static GeneralInputController instance;

        private static MainMenu mainMenuPrefab;
        private const string MainMenuPrefabName = "MainMenu";

        private static MainMenu mainMenuInstance;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            SJResources.LoadComponentOfGameObjectAsync<MainMenu>(MainMenuPrefabName)
                .Subscribe(mainMenu => mainMenuPrefab = mainMenu);

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
                Destroy(mainMenuInstance);
            }
        }

        private static void InstantiateInGame()
        {
            instance = new GameObject(nameof(GeneralInputController)).AddComponent<GeneralInputController>();
            mainMenuInstance = Instantiate(mainMenuPrefab);
            mainMenuInstance.gameObject.SetActive(false);

            UnityUtil.DontDestroyOnLoad(instance.gameObject);
        }

        protected override void SJUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (mainMenuInstance.gameObject.activeSelf)
                {
                    mainMenuInstance.Hide();
                }
                else
                {
                    mainMenuInstance.Show();
                }
            }
        }
    }

}

