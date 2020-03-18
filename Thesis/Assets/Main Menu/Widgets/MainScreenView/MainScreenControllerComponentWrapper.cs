using UnityEngine;

namespace SJ.UI
{
    public class MainScreenControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private MainMenu mainMenu;

        private MainScreenViewController mainScreenController;

        protected override void SJAwake()
        {
            base.SJAwake();

            mainScreenController = new MainScreenViewController(GetComponent<IMainScreenView>(), Application.GameManager, 
                Repositories.GetGameSettingsRepository(), mainMenu, Application.TranslatorService);
        }
    }
}