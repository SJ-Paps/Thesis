using UnityEngine;
using UnityEngine.UI;

namespace SJ.UI
{
    public class LanguageButton : SJMonoBehaviour
    {
        private string language;

        [SerializeField]
        private Button button;

        [SerializeField]
        private Text text;

        protected override void SJAwake()
        {
            button.onClick.AddListener(ChangeLanguage);
        }

        protected override void SJOnEnable()
        {
            Application.GetTranslatorService().onLanguageChanged += OnLanguageChanged;
        }

        protected override void SJOnDisable()
        {
            Application.GetTranslatorService().onLanguageChanged -= OnLanguageChanged;
        }

        public void SetLanguage(string language)
        {
            this.language = language;

            UpdateButtonText();
        }

        private void ChangeLanguage()
        {
            Application.GetTranslatorService().ChangeLanguage(language);

            ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

            gameConfiguration.userLanguage = language;

            GameConfigurationCareTaker.SaveConfiguration();
        }

        private void UpdateButtonText()
        {
            text.text = Application.GetTranslatorService().GetLineByTagOfCurrentLanguage("language_name_" + language).FirstLetterToUpper();
        }

        private void OnLanguageChanged(string language)
        {
            UpdateButtonText();
        }
    }
}


