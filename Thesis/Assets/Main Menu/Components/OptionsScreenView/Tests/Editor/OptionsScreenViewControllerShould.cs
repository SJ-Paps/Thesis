using NSubstitute;
using NUnit.Framework;
using SJ.Menu;
using UnityEngine.Events;

namespace SJ.Tests
{
    public class OptionsScreenViewControllerShould
    {
        private IOptionsScreenView view;
        private IMainMenu mainMenu;

        private OptionsScreenViewController controller;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<IOptionsScreenView>();
            mainMenu = Substitute.For<IMainMenu>();

            controller = new OptionsScreenViewController(view, mainMenu);
        }

        [Test]
        public void Show_Sound_Settings_Screen_When_The_Associated_Button_Is_Clicked()
        {
            view.OnGoToSoundsSettingsButtonClicked += Raise.Event<UnityAction>();

            view.Received(1).FocusSoundSettingsScreen();
        }

        [Test]
        public void Show_Language_Settings_Screen_When_The_Associated_Button_Is_Clicked()
        {
            view.OnGoToLanguageSettingsButtonClicked += Raise.Event<UnityAction>();

            view.Received(1).FocusLanguageSettingsScreen();
        }

        [Test]
        public void Show_Game_Input_Settings_Screen_When_The_Associated_Button_Is_Clicked()
        {
            view.OnGoToGameInputSettingsButtonClicked += Raise.Event<UnityAction>();

            view.Received(1).FocusGameInputSettingsScreen();
        }

        [Test]
        public void Return_To_Main_Screen_When_Back_Button_Is_Clicked()
        {
            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            mainMenu.Received(1).FocusMainScreen();
        }
    }
}