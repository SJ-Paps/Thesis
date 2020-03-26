using SJ.Management;

namespace SJ.Menu
{
    public class NewGameScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        private NewGameScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new NewGameScreenViewController(GetComponent<INewGameScreenView>(), Repositories.GetProfileRepository(),
                Repositories.GetGameSettingsRepository(), Application.TranslatorService, Application.GameManager);
        }
    }
}
