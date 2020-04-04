using SJ.Menu;
using UnityEngine;
using Application = SJ.Management.Application;

namespace SJ
{
    public class GlobalInput : SJMonoBehaviour
    {
        [SerializeField]
        private MainMenu mainMenuPrefab;

        private MainMenu mainMenuInstance;

        protected override void SJAwake()
        {
            gameObject.DontDestroyOnLoad();

            Application.Instance.GameManager().OnSessionFinished += () => { Destroy(mainMenuInstance); mainMenuInstance = null; };
        }

        protected override void SJUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && Application.Instance.GameManager().IsInGame())
            {
                if (mainMenuInstance == null)
                    InstantiateMenu();

                if (mainMenuInstance.gameObject.activeSelf)
                    mainMenuInstance.Hide();
                else
                    mainMenuInstance.Show();
            }
#if UNITY_EDITOR
            else if (Input.GetKeyDown(KeyCode.F5))
                Application.Instance.GameManager().Save();
            else if (Input.GetKeyDown(KeyCode.F6))
                Application.Instance.GameManager().Reload();
#endif
        }

        private void InstantiateMenu()
        {
            mainMenuInstance = Instantiate(mainMenuPrefab);
            mainMenuInstance.gameObject.SetActive(false);
        }
    }

}

