using NSubstitute;
using NUnit.Framework;
using SJ.Localization;
using SJ.Management;
using SJ.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace SJ.Tests
{
    public class LanguageSettingsScreenViewControllerShould
    {
        private const string English = "English";
        private const string Spanish = "Spanish";

        private const string LanguageButtonLocalizedTextTagPrefix = "language_name_";

        private ILanguageSettingsScreenView view;
        private ITranslatorService translatorService;
        private IGameSettingsRepository gameSettingsRepository;
        private IOptionsScreenView optionsScreenView;

        private LanguageSettingsScreenViewController controller;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<ILanguageSettingsScreenView>();
            translatorService = Substitute.For<ITranslatorService>();
            gameSettingsRepository = Substitute.For<IGameSettingsRepository>();
            optionsScreenView = Substitute.For<IOptionsScreenView>();
        }

        [Test]
        public void Create_Language_Buttons_For_All_Languages()
        {
            var languages = GivenATranslatorServiceWithLanguages(English, Spanish);
            GivenAController();

            translatorService.Received(1).GetLineByTagOfCurrentLanguage(LanguageButtonLocalizedTextTagPrefix + English.ToLower());
            translatorService.Received(1).GetLineByTagOfCurrentLanguage(LanguageButtonLocalizedTextTagPrefix + Spanish.ToLower());

            view.Received(1).AddLanguageButtons(Arg.Is<Dictionary<string, string>>(dictionary => dictionary.Keys.All(key => languages.Contains(key))));
        }

        [Test]
        public void Subscribe_To_Any_Language_Selected()
        {
            GivenAController();

            view.Received(1).OnLanguageSelected += Arg.Any<Action<string>>();
        }

        [Test]
        public void Change_Language_When_A_New_Language_Is_Selected()
        {
            GivenAController();

            view.OnLanguageSelected += Raise.Event<Action<string>>(English);

            translatorService.Received(1).ChangeLanguage(English);
        }

        [Test]
        public void Save_New_Language_As_Game_Setting()
        {
            GivenAController();

            var gameSettings = new GameSettings();

            gameSettingsRepository.GetSettings().Returns(Observable.Return(gameSettings));
            gameSettingsRepository.SaveSettings().Returns(Observable.ReturnUnit());

            view.OnLanguageSelected += Raise.Event<Action<string>>(English);

            gameSettingsRepository.Received(1).SaveSettings();

            Assert.That(gameSettings.userLanguage == English, "Has saved user language correctly");
        }

        [Test]
        public void Update_Language_Button_Texts()
        {
            var languages = GivenATranslatorServiceWithLanguages(English, Spanish);
            GivenAController();

            translatorService.ClearReceivedCalls();

            view.OnLanguageSelected += Raise.Event<Action<string>>(English);

            translatorService.Received(1).GetLineByTagOfCurrentLanguage(LanguageButtonLocalizedTextTagPrefix + English.ToLower());
            translatorService.Received(1).GetLineByTagOfCurrentLanguage(LanguageButtonLocalizedTextTagPrefix + Spanish.ToLower());

            view.UpdateLanguageButtonTexts(Arg.Is<Dictionary<string, string>>(dictionary => dictionary.Keys.All(key => languages.Contains(key))));
        }

        private void GivenAController()
        {
            controller = new LanguageSettingsScreenViewController(view, translatorService, gameSettingsRepository, optionsScreenView);
        }

        private string[] GivenATranslatorServiceWithLanguages(params string[] languages)
        {
            translatorService.GetLanguages().Returns(languages);

            return languages;
        }
    }
}