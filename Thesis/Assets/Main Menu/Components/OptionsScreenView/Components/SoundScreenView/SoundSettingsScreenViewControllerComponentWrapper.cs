using SJ.Management;
using UnityEngine;

namespace SJ.Menu
{
    public class SoundSettingsScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private OptionsScreenView optionsScreenView;

        private SoundSettingsScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new SoundSettingsScreenViewController(
                GetComponent<ISoundSettingsScreenView>(), Repositories.GetGameSettingsRepository(),
                Management.Application.SoundService, optionsScreenView);
        }
    }

}