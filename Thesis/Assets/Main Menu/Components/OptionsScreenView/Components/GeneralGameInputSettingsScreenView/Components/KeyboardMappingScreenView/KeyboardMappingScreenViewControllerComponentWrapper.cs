using SJ.Management;
using UnityEngine;

namespace SJ.Menu
{
    public class KeyboardMappingScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private GameInputSettingsScreenView gameInputSettingsScreenView;

        private KeyboardMappingScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new KeyboardMappingScreenViewController(
                GetComponent<IKeyboardMappingScreenView>(), Repositories.GetGameInputSettingsRepository(),
                Management.Application.Instance.TranslatorService(), gameInputSettingsScreenView, Management.Application.Instance.EventBus());
        }
    }
}