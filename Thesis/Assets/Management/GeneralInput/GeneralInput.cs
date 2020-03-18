using Paps.Unity;
using SJ.UI;
using UnityEngine;
using UniRx;

namespace SJ.Management
{
    public class GeneralInput : SJMonoBehaviour
    {
        private static GeneralInput instance;

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
                Application.GameManager.OnLoading += DestroyOnQuittingGameOrLoading;
                Application.GameManager.OnLoadingSucceeded += InstantiateInGame;
                Application.GameManager.OnSessionFinished += DestroyOnQuittingGameOrLoading;
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
            instance = new GameObject(nameof(GeneralInput)).AddComponent<GeneralInput>();
            instance.EnableUpdate = true;
            mainMenuInstance = Instantiate(mainMenuPrefab);
            mainMenuInstance.gameObject.SetActive(false);

            UnityUtil.DontDestroyOnLoad(instance.gameObject);
        }

        protected override void SJUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (mainMenuInstance.gameObject.activeSelf)
                    mainMenuInstance.Hide();
                else
                    mainMenuInstance.Show();
            }

            if (Input.GetKeyDown(KeyCode.F5))
                Application.GameManager.Save();
            else if (Input.GetKeyDown(KeyCode.F6))
                Application.GameManager.Reload();
        }
    }

}

