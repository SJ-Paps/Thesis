using SJ.Management;
using UnityEngine;

namespace SJ.Menu
{
    public class GameInputSettingsScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private OptionsScreenView optionsScreenView;

        private GameInputSettingsScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new GameInputSettingsScreenViewController(
                GetComponent<IGameInputSettingsScreenView>(), Repositories.GetGameInputSettingsRepository(),
                Management.Application.TranslatorService, optionsScreenView);
        }
    }
}