using SJ.Management;
using UnityEngine;

namespace SJ.Menu
{
    public class GeneralGameInputSettingsScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private OptionsScreenView optionsScreenView;

        private GeneralGameInputSettingsScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new GeneralGameInputSettingsScreenViewController(
                GetComponent<IGeneralGameInputSettingsScreenView>(), Repositories.GetGameInputSettingsRepository(),
                Management.Application.TranslatorService, optionsScreenView);
        }
    }
}