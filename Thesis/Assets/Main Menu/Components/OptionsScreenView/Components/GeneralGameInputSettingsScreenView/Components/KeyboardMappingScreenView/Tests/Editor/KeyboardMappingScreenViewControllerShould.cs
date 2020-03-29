using NSubstitute;
using NUnit.Framework;
using SJ.Management;
using SJ.Management.Localization;
using SJ.Menu;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace SJ.Tests
{
    public class KeyboardMappingScreenViewControllerShould
    {
        private const string ResetMessageLanguageTag = "confirmation_menu_message_reset_settings", ResetMessageTranslation = "Confirm Reset",
            SaveConfigurationMessageLanguageTag = "confirmation_menu_message_save_settings", SaveConfigurationMessageTranslation = "Confirm Save";

        private const string KeyGroupName = "SomeKeyGroup";
        private static readonly string KeyGroupNameLanguageTag = "input_action_" + KeyGroupName;
        private const string KeyGroupNameTranslation = "KeyGroupTranslation";

        private const KeyCode KeyGroupMain = KeyCode.A, KeyGroupAlternative = KeyCode.D;

        private const KeyCode ValidRebindKey = KeyCode.R;
        private const KeyCode InvalidRebindKey = KeyCode.Joystick1Button0;
        private const KeyCode CancelRebindKey = KeyCode.Mouse0;
        private const KeyCode RemoveKey = KeyCode.Backspace;

        private GameInputSettings gameInputSettings;

        private IKeyboardMappingScreenView view;
        private ITranslatorService translatorService;
        private IGameInputSettingsRepository gameInputSettingsRepository;
        private IGameInputSettingsScreenView gameInputSettingsScreenView;

        private KeyboardMappingScreenViewController controller;

        [SetUp]
        public void SetUp()
        {
            view = Substitute.For<IKeyboardMappingScreenView>();
            translatorService = Substitute.For<ITranslatorService>();
            gameInputSettingsRepository = Substitute.For<IGameInputSettingsRepository>();
            gameInputSettingsScreenView = Substitute.For<IGameInputSettingsScreenView>();

            translatorService.GetLineByTagOfCurrentLanguage(KeyGroupNameLanguageTag.ToLower()).Returns(KeyGroupNameTranslation);
            translatorService.GetLineByTagOfCurrentLanguage(ResetMessageLanguageTag).Returns(ResetMessageTranslation);
            translatorService.GetLineByTagOfCurrentLanguage(SaveConfigurationMessageLanguageTag).Returns(SaveConfigurationMessageTranslation);
        }

        [Test]
        public void Subscribe_To_Events_On_Construction()
        {
            GivenAKeyGroup();
            GivenAController();

            view.Received(1).OnAppeared += Arg.Any<Action>();
            view.Received(1).OnBackButtonClicked += Arg.Any<UnityAction>();
            view.Received(1).OnKeyListened += Arg.Any<Action<KeyCode>>();
            view.Received(1).OnSaveButtonClicked += Arg.Any<UnityAction>();
            view.Received(1).OnRequestedMainKeyRebind += Arg.Any<Action<string>>();
            view.Received(1).OnRequestedAlternativeKeyRebind += Arg.Any<Action<string>>();
            view.Received(1).OnResetButtonClicked += Arg.Any<UnityAction>();
        }

        [Test]
        public void Create_Input_Key_Group_Controls_That_Represents_The_Ones_In_Game_Input_Settings()
        {
            GivenAKeyGroup();
            GivenAController();

            view.Received(1).CreateInputKeyGroupControl(
                KeyGroupName, KeyGroupNameTranslation,
                KeyGroupMain, KeyGroupAlternative,
                DisplayNameFor(KeyGroupMain), DisplayNameFor(KeyGroupAlternative));
        }

        [Test]
        public void Refresh_Key_Group_Controls_When_View_Appears()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnAppeared += Raise.Event<Action>();

            view.Received(1).UpdateMainKeyOf(KeyGroupName, KeyGroupMain, DisplayNameFor(KeyGroupMain));
            view.Received(1).UpdateAlternativeKeyOf(KeyGroupName, KeyGroupAlternative, DisplayNameFor(KeyGroupAlternative));
        }

        [Test]
        public void Listen_Key_Input_When_Receives_A_Rebind_Request()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);
            view.OnRequestedAlternativeKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.Received(2).ListenForInputs();
        }

        [Test]
        public void Show_As_Modifiable_Main_Or_Alternative_Of_Requested_Key_Group_To_Rebind()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);
            view.OnRequestedAlternativeKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.Received(1).ShowAsModifiableMainOf(KeyGroupName);
            view.Received(1).ShowAsModifiableAlternativeOf(KeyGroupName);
        }

        [Test]
        public void Lock_Interactable_Objects_When_Receives_A_Rebind_Request()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);
            view.OnRequestedAlternativeKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.Received(2).LockInteractableObjects();
        }

        [Test]
        public void Rebind_Corresponding_Key_Group_Key_As_A_Backup_When_Receives_A_Valid_Key()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.Received(1).UpdateMainKeyOf(KeyGroupName, ValidRebindKey, DisplayNameFor(ValidRebindKey));

            view.OnRequestedAlternativeKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.Received(1).UpdateAlternativeKeyOf(KeyGroupName, ValidRebindKey, DisplayNameFor(ValidRebindKey));

            gameInputSettingsRepository.DidNotReceive().SaveSettings();
        }

        [Test]
        public void Do_Nothing_When_Receives_An_Invalid_Key()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(InvalidRebindKey);

            view.OnRequestedAlternativeKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(InvalidRebindKey);

            view.DidNotReceiveWithAnyArgs().UpdateMainKeyOf(default, default, default);
        }

        [Test]
        public void Stop_Rebinding_When_Receives_A_Valid_Key()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.OnRequestedAlternativeKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            AssertHasStoppedRebinding(2);
        }

        [Test]
        public void Stop_Rebinding_When_Receives_A_Cancel_Rebind_Key()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(CancelRebindKey);

            view.OnRequestedAlternativeKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(CancelRebindKey);

            AssertHasStoppedRebinding(2);
        }

        [Test]
        public void Rebind_With_KeyCode_None_When_Receives_A_Remove_Key()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(RemoveKey);

            view.OnRequestedAlternativeKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(RemoveKey);

            view.Received(1).UpdateMainKeyOf(KeyGroupName, KeyCode.None, DisplayNameFor(KeyCode.None));
            view.Received(1).UpdateAlternativeKeyOf(KeyGroupName, KeyCode.None, DisplayNameFor(KeyCode.None));

            AssertHasStoppedRebinding(2);
        }

        [Test]
        public void Save_Configuration_When_Save_Button_Is_Clicked_And_Settings_Have_Changed()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.OnSaveButtonClicked += Raise.Event<UnityAction>();

            gameInputSettingsRepository.Received(1).SaveSettings();

            var expectedChangedGroup = gameInputSettings.GetGroups()[KeyGroupName];

            Assert.That(expectedChangedGroup.main == ValidRebindKey, "Game input settings have changed");
        }

        [Test]
        public void Show_Confirmation_Popup_When_Reset_Button_Is_Clicked_And_Settings_Have_Changed()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.OnResetButtonClicked += Raise.Event<UnityAction>();

            view.Received(1).ShowConfirmationPopup(ResetMessageTranslation, Arg.Any<Action>(), Arg.Any<Action>());
        }

        [Test]
        public void Do_Nothing_When_Reset_Button_Is_Clicked_And_Settings_Have_Not_Changed()
        {
            GivenAKeyGroup();
            GivenAController();

            view.ClearReceivedCalls();

            view.OnResetButtonClicked += Raise.Event<UnityAction>();

            view.DidNotReceiveWithAnyArgs().ShowConfirmationPopup(default, default, default);
        }

        [Test]
        public void Reset_Settings_When_Confirmation_Popup_Is_Accepted()
        {
            GivenAKeyGroup();
            GivenAController();
            GivenAConfirmationPopupThatWillBeAccepted();

            view.ClearReceivedCalls();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.ClearReceivedCalls();

            view.OnResetButtonClicked += Raise.Event<UnityAction>();

            view.Received(1).UpdateMainKeyOf(KeyGroupName, KeyGroupMain, DisplayNameFor(KeyGroupMain));
            view.Received(1).UpdateAlternativeKeyOf(KeyGroupName, KeyGroupAlternative, DisplayNameFor(KeyGroupAlternative));
        }

        [Test]
        public void Show_Confirmation_Popup_Asking_If_User_Wants_To_Save_Configuration_When_Back_Button_Is_Clicked_And_Settings_Have_Changed()
        {
            GivenAKeyGroup();
            GivenAController();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            view.Received(1).ShowConfirmationPopup(SaveConfigurationMessageTranslation, Arg.Any<Action>(), Arg.Any<Action>());
        }

        [Test]
        public void Go_Back_When_Back_Button_Is_Clicked_And_Settings_Have_Not_Changed()
        {
            GivenAKeyGroup();
            GivenAController();

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            view.DidNotReceiveWithAnyArgs().ShowConfirmationPopup(default, default, default);

            gameInputSettingsScreenView.Received(1).FocusMainScreen();
        }

        [Test]
        public void Save_Configuration_And_Go_Back_When_Confirmation_Popup_Is_Accepted()
        {
            GivenAKeyGroup();
            GivenAController();
            GivenAConfirmationPopupThatWillBeAccepted();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            Received.InOrder(() =>
            {
                gameInputSettingsRepository.SaveSettings();
                gameInputSettingsScreenView.FocusMainScreen();
            });
        }

        [Test]
        public void Simply_Go_Back_When_Confirmation_Popup_Is_Cancelled()
        {
            GivenAKeyGroup();
            GivenAController();
            GivenAConfirmationPopupThatWillBeCancelled();

            view.OnRequestedMainKeyRebind += Raise.Event<Action<string>>(KeyGroupName);

            view.OnKeyListened += Raise.Event<Action<KeyCode>>(ValidRebindKey);

            view.OnBackButtonClicked += Raise.Event<UnityAction>();

            gameInputSettingsScreenView.Received(1).FocusMainScreen();
        }

        private void GivenAKeyGroup()
        {
            gameInputSettings = ScriptableObject.CreateInstance<GameInputSettings>();

            gameInputSettings.AddGroup(KeyGroupName, KeyGroupMain, KeyGroupAlternative);

            gameInputSettingsRepository.GetSettings().Returns(Observable.Return(gameInputSettings).ObserveOn(Scheduler.Immediate));
            gameInputSettingsRepository.SaveSettings().Returns(Observable.ReturnUnit().ObserveOn(Scheduler.Immediate));
        }

        private void GivenAController()
        {
            controller = new KeyboardMappingScreenViewController(view, gameInputSettingsRepository, translatorService, gameInputSettingsScreenView);
        }

        private void GivenAConfirmationPopupThatWillBeAccepted()
        {
            int acceptCallbackPosition = 1;

            view.WhenForAnyArgs(v => v.ShowConfirmationPopup(default, default, default))
                .Do(callbackInfo => callbackInfo.ArgAt<Action>(acceptCallbackPosition).Invoke());
        }

        private void GivenAConfirmationPopupThatWillBeCancelled()
        {
            int cancelCallbackPosition = 2;

            view.WhenForAnyArgs(v => v.ShowConfirmationPopup(default, default, default))
                .Do(callbackInfo => callbackInfo.ArgAt<Action>(cancelCallbackPosition).Invoke());
        }

        private string DisplayNameFor(KeyCode key) => key.ToString();

        private void AssertHasStoppedRebinding(int times)
        {
            view.Received(times).StopListeningInputs();
            view.Received(times).ShowAllAsNotModifiable();
            view.Received(times).UnlockInteractableObjects();
        }
    }
}