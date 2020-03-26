using SJ.Localization;
using SJ.Management;
using System.Collections.Generic;
using UniRx;

namespace SJ.Menu
{
    public class LanguageMenuViewController
    {
        private const string LanguageButtonTextsTagPrefix = "language_name_";

        private ILanguageMenuView view;
        private ITranslatorService translatorService;
        private IGameSettingsRepository gameSettingsRepository;

        private string[] languages;

        public LanguageMenuViewController(ILanguageMenuView view, ITranslatorService translatorService, IGameSettingsRepository gameSettingsRepository)
        {
            this.view = view;
            this.translatorService = translatorService;
            this.gameSettingsRepository = gameSettingsRepository;

            languages = translatorService.GetLanguages();

            view.OnLanguageSelected += ChangeAndSaveLanguage;
            view.AddLanguageButtons(GetTranslatedButtonTexts(languages));
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

