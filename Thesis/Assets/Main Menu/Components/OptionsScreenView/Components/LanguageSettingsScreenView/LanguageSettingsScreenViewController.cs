using SJ.Management.Localization;
using SJ.Management;
using System.Collections.Generic;
using UniRx;

namespace SJ.Menu
{
    public class LanguageSettingsScreenViewController
    {
        private const string LanguageButtonTextsTagPrefix = "language_name_";

        private ILanguageSettingsScreenView view;
        private ITranslatorService translatorService;
        private IGameSettingsRepository gameSettingsRepository;
        private IOptionsScreenView optionsScreenView;

        private string[] languages;

        public LanguageSettingsScreenViewController(ILanguageSettingsScreenView view, ITranslatorService translatorService,
            IGameSettingsRepository gameSettingsRepository, IOptionsScreenView optionsScreenView)
        {
            this.view = view;
            this.translatorService = translatorService;
            this.gameSettingsRepository = gameSettingsRepository;
            this.optionsScreenView = optionsScreenView;

            Initialize();
        }

        private void Initialize()
        {
            languages = translatorService.GetLanguages();

            view.OnLanguageSelected += ChangeAndSaveLanguage;
            view.AddLanguageButtons(GetTranslatedButtonTexts(languages));
            view.OnBackButtonClicked += optionsScreenView.FocusMainScreen;
        }

        private void ChangeAndSaveLanguage(string language)
        {
            translatorService.ChangeLanguage(language);

            view.UpdateLanguageButtonTexts(GetTranslatedButtonTexts(languages));

            gameSettingsRepository.GetSettings()
                .Do(gameSettings => gameSettings.userLanguage = language)
                .ContinueWith(_ => gameSettingsRepository.SaveSettings())
                .Subscribe();
        }

        private Dictionary<string, string> GetTranslatedButtonTexts(string[] languages)
        {
            var translatedButtonTexts = new Dictionary<string, string>();

            foreach (var language in languages)
                translatedButtonTexts.Add(language, 
                    translatorService.GetLineByTagOfCurrentLanguage((LanguageButtonTextsTagPrefix + language)
                    .ToLower())
                    .ToTitleCase()
                    );

            return translatedButtonTexts;
        }
    }
}

