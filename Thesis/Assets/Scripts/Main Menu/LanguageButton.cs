using UnityEngine;
using UnityEngine.UI;
using UniRx;

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

            Repositories.GetGameSettingsRepository().GetSettings()
                .Subscribe(gameSettings =>
                {
                    gameSettings.userLanguage = language;

                    Repositories.GetGameSettingsRepository().SaveSettings().Subscribe();
                });
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


