using NUnit.Framework;
using SJ.Management;
using SJ.UI;
using NSubstitute;
using System;
using UniRx;
using UnityEngine.Events;

namespace SJ.Tests
{
    public class MainScreenViewControllerShould
    {
        private const string LastProfile = "SomeProfile";

        private IMainScreenView view;
        private IGameManager gameManager;
        private IGameSettingsRepository gameSettingsRepository;
        private IMainMenu mainMenu;

        private MainScreenViewController controller;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<IMainScreenView>();
            gameManager = Substitute.For<IGameManager>();
            gameSettingsRepository = Substitute.For<IGameSettingsRepository>();
            mainMenu = Substitute.For<IMainMenu>();

            controller = new MainScreenViewController(view, gameManager, gameSettingsRepository, mainMenu);
        }

        [Test]
        public void Subscribe_To_On_Appeared()
        {
            view.Received(1).OnAppeared += Arg.Any<Action>();
        }

        [Test]
        public void Show_In_Game_If_Is_In_Game()
        {
            gameManager.IsInGame().Returns(true);

            view.OnAppeared += Raise.Event<Action>();

            gameManager.Received(1).IsInGame();

            view.Received(1).ShowInGameButtons();
        }

        [Test]
        public void Show_In_Menu_If_Is_Not_In_Game()
        {
            gameManager.IsInGame().Returns(false);

            view.OnAppeared += Raise.Event<Action>();

            view.Received(1).ShowInMenuButtons();
        }

        [Test]
        public void Show_Continue_Button_If_Is_Not_In_Game_And_Last_Profile_Exists()
        {
            gameManager.IsInGame().Returns(false);

            gameSettingsRepository.GetSettings().Returns(Observable.Return(new GameSettings() { lastProfile = LastProfile }));

            view.OnAppeared += Raise.Event<Action>();

            view.Received(1).ShowContinueButton();
        }

        [Test]
        public void Hide_Continue_Button_If_Last_Profile_Is_Null_Or_Empty()
        {
            gameManager.IsInGame().Returns(false);

            gameSettingsRepository.GetSettings().Returns(Observable.Return(new GameSettings()));

            view.OnAppeared += Raise.Event<Action>();

            view.Received(1).HideContinueButton();
        }

        [Test]
        public void Hide_Continue_Button_If_Is_In_Game()
        {
            gameManager.IsInGame().Returns(true);

            view.OnAppeared += Raise.Event<Action>();

            view.Received(1).HideContinueButton();
        }
        
        [Test]
        public void Subscribe_To_Focus_Screen_Buttons()
        {
            view.Received(1).OnNewGameClicked += Arg.Any<UnityAction>();
            view.Received(1).OnLoadGameClicked += Arg.Any<UnityAction>();
            view.Received(1).OnOptionsClicked += Arg.Any<UnityAction>();
        }

        [Test]
        public void Focus_New_Game_Screen_When_The_Button_Is_Clicked()
        {
            view.OnNewGameClicked += Raise.Event<UnityAction>();

            mainMenu.Received(1).FocusNewGameScreen();
        }

        [Test]
        public void Focus_Load_Game_Screen_When_The_Button_Is_Clicked()
        {
            view.OnLoadGameClicked += Raise.Event<UnityAction>();

            mainMenu.Received(1).FocusLoadGameScreen();
        }

        [Test]
        public void Focus_Options_Screen_When_The_Button_Is_Clicked()
        {
            view.OnOptionsClicked += Raise.Event<UnityAction>();

            mainMenu.Received(1).FocusOptionsScreen();
        }

        [Test]
        public void Hide_Main_Menu_When_Resume_Game_Button_Is_Clicked()
        {
            view.OnResumeGameClicked += Raise.Event<UnityAction>();

            mainMenu.Received(1).Hide();
        }
    }
}