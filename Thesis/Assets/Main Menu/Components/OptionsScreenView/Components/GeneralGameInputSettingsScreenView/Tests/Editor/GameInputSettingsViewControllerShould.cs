using NSubstitute;
using NUnit.Framework;
using Paps.EventBus;
using SJ.Management;
using SJ.Management.Localization;
using SJ.Menu;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace SJ.Tests
{
    public class GameInputSettingsViewControllerShould
    {
        private const string SaveConfirmationMessageLanguageTag = "confirmation_menu_message_save_settings",
            SaveConfirmationMessageTranslation = "SomeMessage";

        private IGameInputSettingsScreenView view;
        private IGameInputSettingsRepository gameInputSettingsRepository;
        private ITranslatorService translatorService;
        private IOptionsScreenView optionsScreenView;
        private IEventBus eventBus;

        private GameInputSettings gameInputSettings;

        private GameInputSettingsScreenViewController controller;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<IGameInputSettingsScreenView>();
            gameInputSettingsRepository = Substitute.For<IGameInputSettingsRepository>();
            translatorService = Substitute.For<ITranslatorService>();
            optionsScreenView = Substitute.For<IOptionsScreenView>();
            eventBus = Substitute.For<IEventBus>();

            gameInputSettings = ScriptableObject.CreateInstance<GameInputSettings>();

            gameInputSettingsRepository.GetSettings().Returns(Observable.Return(gameInputSettings).ObserveOn(Scheduler.Immediate));

            translatorService.GetLineByTagOfCurrentLanguage(SaveConfirmationMessageLanguageTag).Returns(SaveConfirmationMessageTranslation);
        }

        [Test]
        public void Subscribe_To_Events_On_Construction()
        {
            GivenAController();

            view.Received(1).OnHoldDuckKeyToggleChanged += Arg.Any<UnityAction<bool>>();
            view.Received(1).OnHoldWalkKeyToggleChanged += Arg.Any<UnityAction<bool>>();

            view.Received(1).OnAppeared += Arg.Any<Action>();

            view.Received(1).OnBackButtonClicked += Arg.Any<UnityAction>();
            view.Received(1).OnSaveButtonClicked += Arg.Any<UnityAction>();

            view.Received(1).OnGoToKeyboardMappingClicked += Arg.Any<UnityAction>();
            view.Received(1).OnGoToJoystickMappingClicked += Arg.Any<UnityAction>();
        }

        [Test]
        public void Refresh_Settings_On_Construction()
        {
            gameInputSettings.holdDuckKey = true;
            gameInputSettings.holdWalkKey = true;

            gameInputSettingsRepository.ClearReceivedCalls();

            GivenAController();

            gameInputSettingsRepository.Received(1).GetSettings();

            view.Received(1).SetHoldDuckKeyToggleValue(gameInputSettings.holdDuckKey);
            view.Received(1).SetHoldWalkKeyToggleValue(gameInputSettings.holdWalkKey);
        }

        [Test]
        public void Save_Configuration_When_Save_Button_Is_Clicked_And_User_Has_Changed_Settings()
        {
            GivenAController();

            view.OnHoldDuckKeyToggleChanged += Raise.Event<UnityAction<bool>>(true);

            view.OnSaveButtonClicked += Raise.Event<UnityAction>();

            gameInputSettingsRepository.Received(1).SaveSettings();

            Assert.That(gameInputSettings.holdDuckKey == true, "Hold duck key has changed");

            view.OnHoldWalkKeyToggleChanged += Raise.Event<UnityAction<bool>>(true);

            gameInputSettingsRepository.ClearReceivedCalls();

            view.OnSaveButtonClicked += Raise.Event<UnityAction>();

            gameInputSettingsRepository.Received(1).SaveSettings();

            Assert.That(gameInputSettings.holdWalkKey == true, "Hold walk key has changed");
        }

        [Test]
        public void Do_Nothing_If_Save_Button_Is_Clicked_And_User_Has_Not_Changed_Anything()
        {
            GivenAController();

            view.OnSaveButtonClicked += Raise.Event<UnityAction>();

            gameInputSettingsRepository.DidNotReceive().SaveSettings();
        }

        [Test]
        public void Refresh_Settings_On_Appeared()
        {
            gameInputSettings.holdDuckKey = true;
            gameInputSettings.holdWalkKey = true;

            GivenAController();

            gameInputSettingsRepository.ClearReceivedCalls();
            view.ClearReceivedCalls();

            view.OnAppeared += Raise.Event<Action>();

            gameInputSettingsRepository.Received(1).GetSettings();

            view.Received(1).SetHoldDuckKeyToggleValue(gameInputSettings.holdDuckKey);
            view.Received(1).SetHoldWalkKeyToggleValue(gameInputSettings.holdWalkKey);
        }

        [Test]
        public void Show_Confirmation_Popup_When_Back_Button_Is_Clicked_And_User_Has_Changed_Settings_Without_Been_Saved()
        {
            GivenAController();

            view.OnHoldDuckKeyToggleChanged += Raise.Event<UnityAction<bool>>(true);

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            view.Received(1).ShowApplyConfirmationPopup(SaveConfirmationMessageTranslation, Arg.Any<Action>(), Arg.Any<Action>());
        }

        [Test]
        public void Save_Configuration_Before_Go_To_Main_Screen_If_User_Accepts_Confirmation_Popup()
        {
            GivenAConfirmationPopupThatWillBeAccepted();

            GivenAController();

            gameInputSettingsRepository.ClearReceivedCalls();

            view.OnHoldDuckKeyToggleChanged += Raise.Event<UnityAction<bool>>(true);

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            Received.InOrder(() =>
            {
                gameInputSettingsRepository.SaveSettings();
                optionsScreenView.FocusMainScreen();
            });
        }
        
        [Test]
        public void Simply_Go_To_Main_Screen_If_User_Cancels_Confirmation_Popup()
        {
            GivenAConfirmationPopupThatWillBeCancelled();

            GivenAController();

            gameInputSettingsRepository.ClearReceivedCalls();

            view.OnHoldDuckKeyToggleChanged += Raise.Event<UnityAction<bool>>(true);

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            gameInputSettingsRepository.DidNotReceive().SaveSettings();

            optionsScreenView.Received(1).FocusMainScreen();
        }

        [Test]
        public void Go_To_Main_Screen_When_Back_Button_Is_Clicked_And_No_Change_Has_Been_Made()
        {
            GivenAController();

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            view.DidNotReceiveWithAnyArgs().ShowApplyConfirmationPopup(default, default, default);

            optionsScreenView.Received(1).FocusMainScreen();
        }

        [Test]
        public void Go_To_Keyboard_Mapping_Screen_If_Associated_Button_Is_Clicked()
        {
            GivenAController();

            view.OnGoToKeyboardMappingClicked += Raise.Event<UnityAction>();

            view.Received(1).FocusKeyboardMappingScreen();
        }

        [Test]
        public void Go_To_Joystick_Mapping_Screen_If_Associated_Button_Is_Clicked()
        {
            GivenAController();

            view.OnGoToJoystickMappingClicked += Raise.Event<UnityAction>();

            view.Received(1).FocusJoystickMappingScreen();
        }

        [Test]
        public void Publish_Game_Input_Settings_Changed_Event_When_Configuration_Is_Changed()
        {
            GivenAController();

            view.OnHoldDuckKeyToggleChanged += Raise.Event<UnityAction<bool>>(true);

            view.OnSaveButtonClicked += Raise.Event<UnityAction>();

            eventBus.Received(1).Publish(ApplicationEvents.GameInputSettingsChanged);
        }

        private void GivenAController()
        {
            controller = new GameInputSettingsScreenViewController(view, gameInputSettingsRepository, translatorService, optionsScreenView, eventBus);
        }

        private void GivenAConfirmationPopupThatWillBeAccepted()
        {
            int acceptCallbackPosition = 1;

            view.WhenForAnyArgs(v => v.ShowApplyConfirmationPopup(default, default, default))
                .Do(callbackInfo => callbackInfo.ArgAt<Action>(acceptCallbackPosition).Invoke());
        }

        private void GivenAConfirmationPopupThatWillBeCancelled()
        {
            int cancelCallbackPosition = 2;

            view.WhenForAnyArgs(v => v.ShowApplyConfirmationPopup(default, default, default))
                .Do(callbackInfo => callbackInfo.ArgAt<Action>(cancelCallbackPosition).Invoke());
        }
    }
}