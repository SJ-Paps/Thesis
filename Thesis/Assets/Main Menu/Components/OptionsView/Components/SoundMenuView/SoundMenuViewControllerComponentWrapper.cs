using SJ.Management;

namespace SJ.Menu
{
    public class SoundMenuViewControllerComponentWrapper : SJMonoBehaviour
    {
        private SoundMenuViewController controller;

        protected override void SJAwake()
        {
            controller = new SoundMenuViewController(GetComponent<ISoundMenuView>(), Repositories.GetGameSettingsRepository(), Application.SoundService);
        }
    }

}