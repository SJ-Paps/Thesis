using Paps.Unity;
using SJ.Menu;
using UnityEngine;
using UniRx;

namespace SJ.Management
{
    public class GlobalInGameInput : SJMonoBehaviour
    {
        private static GlobalInGameInput instance;

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
            instance = new GameObject(nameof(GlobalInGameInput)).AddComponent<GlobalInGameInput>();
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

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F5))
                Application.GameManager.Save();
            else if (Input.GetKeyDown(KeyCode.F6))
                Application.GameManager.Reload();
#endif
        }
    }

}

