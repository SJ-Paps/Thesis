using NSubstitute;
using NUnit.Framework;
using SJ.Management;
using SJ.Menu;
using System;
using System.Linq;
using UniRx;

namespace SJ.Tests
{
    public class LoadGameScreenViewControllerShould
    {
        private const string Profile1 = "Profile1", Profile2 = "Profile2";

        private ILoadGameScreenView view;
        private IProfileRepository profileRepository;
        private IGameSettingsRepository gameSettingsRepository;
        private IGameManager gameManager;
        private IMainMenu mainMenu;

        private LoadGameScreenViewController controller;

        private string[] profiles;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<ILoadGameScreenView>();
            profileRepository = Substitute.For<IProfileRepository>();
            gameSettingsRepository = Substitute.For<IGameSettingsRepository>();
            gameManager = Substitute.For<IGameManager>();
            mainMenu = Substitute.For<IMainMenu>();
        }

        [Test]
        public void Subscribe_To_Events()
        {
            GivenAController();

            view.Received(1).OnAppeared += Arg.Any<Action>();
            view.Received(1).OnProfileSelectClicked += Arg.Any<Action<string>>();
            view.Received(1).OnProfileDeleteClicked += Arg.Any<Action<string>>();
        }

        [Test]
        public void Update_Profiles_When_Is_Created()
        {
            GivenTheseSavedProfiles(Profile1, Profile2);

            GivenAController();

            profileRepository.Received(1).GetAllProfiles();
            view.Received(1).ShowProfiles(profiles);
        }

        [Test]
        public void Update_Profiles_When_View_Appears()
        {
            GivenTheseSavedProfiles(Profile1, Profile2);

            GivenAController();

            view.ClearReceivedCalls();
            profileRepository.ClearReceivedCalls();

            view.OnAppeared += Raise.Event<Action>();

            profileRepository.Received(1).GetAllProfiles();
            view.Received(1).ShowProfiles(profiles);
        }

        [Test]
        public void Clear_Last_Profile_If_User_Requests_To_Delete_That_Profile()
        {
            GivenTheseSavedProfiles(Profile1, Profile2);

            GivenAController();

            var gameSettings = new GameSettings();
            gameSettings.lastProfile = Profile1;

            gameSettingsRepository.GetSettings().Returns(Observable.Return(gameSettings));

            view.OnProfileDeleteClicked += Raise.Event<Action<string>>(Profile1);

            gameSettingsRepository.Received(1).GetSettings();

            Assert.That(string.IsNullOrEmpty(gameSettings.lastProfile), "Last profile has been cleared");
        }

        [Test]
        public void Delete_Profile_If_User_Requested_So()
        {
            GivenTheseSavedProfiles(Profile1, Profile2);

            GivenAController();

            gameSettingsRepository.GetSettings().Returns(Observable.Return(new GameSettings()));

            view.OnProfileDeleteClicked += Raise.Event<Action<string>>(Profile1);

            profileRepository.Received(1).DeleteProfile(Profile1);
        }

        [Test]
        public void Update_Profiles_After_Deletion()
        {
            GivenTheseSavedProfiles(Profile1, Profile2);

            GivenAController();

            gameSettingsRepository.GetSettings().Returns(Observable.Return(new GameSettings()));
            profileRepository.DeleteProfile(Profile1).Returns(Observable.ReturnUnit());

            profileRepository.When(repository => repository.DeleteProfile(Profile1)).Do(callback => GivenTheseSavedProfiles(Profile2));

            view.ClearReceivedCalls();

            view.OnProfileDeleteClicked += Raise.Event<Action<string>>(Profile1);

            view.Received(1).ShowProfiles(Arg.Is<string[]>(array => array.Contains(Profile1) == false && array.Contains(Profile2)));
        }

        private void GivenAController()
        {
            controller = new LoadGameScreenViewController(view, profileRepository, gameSettingsRepository, gameManager, mainMenu);
        }

        private void GivenTheseSavedProfiles(params string[] profiles)
        {
            this.profiles = profiles;

            profileRepository.GetAllProfiles().Returns(Observable.Return(profiles));
        }
    }
}