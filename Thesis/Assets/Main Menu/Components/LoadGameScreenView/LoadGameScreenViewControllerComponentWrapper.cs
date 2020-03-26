using SJ.Management;

namespace SJ.Menu
{
    public class LoadGameScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        private LoadGameScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new LoadGameScreenViewController(GetComponent<ILoadGameScreenView>(), Repositories.GetProfileRepository(), 
                Repositories.GetGameSettingsRepository(), Application.GameManager);
        }
    }
}