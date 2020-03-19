using NSubstitute;
using NUnit.Framework;
using SJ.Localization;
using SJ.Management;
using SJ.Menu;
using SJ.Profiles;
using System;
using UniRx;

namespace SJ.Tests
{
    public class NewGameScreenViewControllerShould
    {
        private const string NewProfile = "NewProfile";

        private const string InvalidProfileNameLanguageTag = "notification_profile_invalid_name", InvalidProfileNameTranslation = "Invalid Profile Name";
        private const string ProfileInUseLanguageTag = "notification_profile_in_use", ProfileInUseTranslation = "Profile is in use";

        private INewGameScreenView view;
        private IProfileRepository profileRepository;
        private IGameSettingsRepository gameSettingsRepository;
        private ITranslatorService translatorService;
        private IGameManager gameManager;

        private NewGameScreenViewController controller;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<INewGameScreenView>();
            profileRepository = Substitute.For<IProfileRepository>();
            gameSettingsRepository = Substitute.For<IGameSettingsRepository>();
            translatorService = Substitute.For<ITranslatorService>();
            gameManager = Substitute.For<IGameManager>();

            translatorService.GetLineByTagOfCurrentLanguage(InvalidProfileNameLanguageTag).Returns(InvalidProfileNameTranslation);
            translatorService.GetLineByTagOfCurrentLanguage(ProfileInUseLanguageTag).Returns(ProfileInUseTranslation);

            controller = new NewGameScreenViewController(view, profileRepository, gameSettingsRepository, translatorService, gameManager);
        }

        [Test]
        public void Subscribe_To_New_Profile_Submitted()
        {
            view.Received(1).OnNewProfileSubmitted += Arg.Any<Action<string>>();
        }

        [Test]
        public void Show_Corresponding_Error_Message_If_New_Profile_Is_Empty()
        {
            view.OnNewProfileSubmitted += Raise.Event<Action<string>>(string.Empty);

            view.Received(1).ShowErrorMessage(InvalidProfileNameTranslation);
        }

        [Test]
        public void Show_Corresponding_Error_Message_If_New_Profile_Is_In_Use()
        {
            profileRepository.Exists(NewProfile).Returns(Observable.Return(true));

            view.OnNewProfileSubmitted += Raise.Event<Action<string>>(NewProfile);

            profileRepository.Received(1).Exists(NewProfile);

            view.Received(1).ShowErrorMessage(ProfileInUseTranslation);
        }

        [Test]
        public void Hide_Error_Message_When_Starts_Processing_Input()
        {
            view.OnNewProfileSubmitted += Raise.Event<Action<string>>(NewProfile);

            view.Received(1).HideErrorMessage();
        }

        [Test]
        public void Save_New_Profile_On_Game_Settings_If_It_Is_Valid()
        {
            var gameSettings = new GameSettings();

            gameSettingsRepository.GetSettings().Returns(Observable.Return(gameSettings));
            gameSettingsRepository.SaveSettings().Returns(Observable.ReturnUnit());

            view.OnNewProfileSubmitted += Raise.Event<Action<string>>(NewProfile);

            gameSettingsRepository.Received(1).GetSettings();
            gameSettingsRepository.Received(1).SaveSettings();

            Assert.That(gameSettings.lastProfile == NewProfile, "Last profile has been updated");
        }

        [Test]
        public void Begin_Session_For_New_Profile()
        {
            gameSettingsRepository.GetSettings().Returns(Observable.Return(new GameSettings()));
            gameSettingsRepository.SaveSettings().Returns(Observable.ReturnUnit());

            view.OnNewProfileSubmitted += Raise.Event<Action<string>>(NewProfile);

            gameManager.Received(1).BeginSessionFor(NewProfile);
        }
    }
}