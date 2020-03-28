using SJ.Management;
using UnityEngine;

namespace SJ.Menu
{
    public class LanguageSettingsScreenViewControllerComponentWrapper : SJMonoBehaviour
    {
        [SerializeField]
        private OptionsScreenView optionsScreenView;

        private LanguageSettingsScreenViewController controller;

        protected override void SJAwake()
        {
            controller = new LanguageSettingsScreenViewController(
                GetComponent<ILanguageSettingsScreenView>(), Management.Application.TranslatorService, 
                Repositories.GetGameSettingsRepository(), optionsScreenView);
        }
    }
}

