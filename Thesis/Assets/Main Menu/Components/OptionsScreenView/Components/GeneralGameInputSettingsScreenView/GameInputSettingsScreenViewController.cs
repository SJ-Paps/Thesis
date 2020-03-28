using SJ.Management;
using UniRx;
using System;
using SJ.Management.Localization;

namespace SJ.Menu
{
    public class GameInputSettingsScreenViewController
    {
        private struct MutableSettings
        {
            public bool holdDuckKey;
            public bool holdWalkKey;
        }

        private const string SaveConfirmationMessageLanguageTag = "confirmation_menu_message_save_settings";

        private IGameInputSettingsScreenView view;
        private IGameInputSettingsRepository gameInputSettingsRepository;
        private IOptionsScreenView optionsScreenView;
        private ITranslatorService translatorService;

        private MutableSettings mutableSettings;

        private bool settingsDirtyFlag;

        public GameInputSettingsScreenViewController(IGameInputSettingsScreenView view, IGameInputSettingsRepository gameInputSettingsRepository,
            ITranslatorService translatorService, IOptionsScreenView optionsScreenView)
        {
            this.view = view;
            this.gameInputSettingsRepository = gameInputSettingsRepository;
            this.translatorService = translatorService;
            this.optionsScreenView = optionsScreenView;

            Initialize();
        }

        private void Initialize()
        {
            view.OnHoldDuckKeyToggleChanged += UpdateHoldDuckKeyValue;
            view.OnHoldWalkKeyToggleChanged += UpdateHoldWalkKeyValue;

            view.OnAppeared += RefreshSettings;

            view.OnBackButtonClicked += RequestSaveConfirmationIfSettingsHaveChangedOrGoBack;
            view.OnSaveButtonClicked += Apply;

            view.OnGoToKeyboardMappingClicked += view.FocusKeyboardMappingScreen;
            view.OnGoToJoystickMappingClicked += view.FocusJoystickMappingScreen;

            RefreshSettings();
        }

        private void RequestSaveConfirmationIfSettingsHaveChangedOrGoBack()
        {
            if (settingsDirtyFlag)
                RequestSaveConfirmation(ApplyAndGoBack, GoBack);
            else
                GoBack();
        }

        private void RequestSaveConfirmation(Action onAccept, Action onCancel)
        {
            view.ShowApplyConfirmationPopup(translatorService.GetLineByTagOfCurrentLanguage(SaveConfirmationMessageLanguageTag), onAccept, onCancel);
        }

        private void RefreshSettings()
        {
            settingsDirtyFlag = false;

            gameInputSettingsRepository.GetSettings()
                .Do(DisplaySavedSettings)
                .Do(ResetMutableSettings)
                .Subscribe();
        }

        private void DisplaySavedSettings(GameInputSettings settings)
        {
            view.SetHoldDuckKeyToggleValue(settings.holdDuckKey);
            view.SetHoldWalkKeyToggleValue(settings.holdWalkKey);
        }

        private void ResetMutableSettings(GameInputSettings settings)
        {
            mutableSettings.holdDuckKey = settings.holdDuckKey;
            mutableSettings.holdWalkKey = settings.holdWalkKey;
        }

        private void UpdateHoldDuckKeyValue(bool value)
        {
            settingsDirtyFlag = true;
            mutableSettings.holdDuckKey = value;
        }

        private void UpdateHoldWalkKeyValue(bool value)
        {
            settingsDirtyFlag = true;
            mutableSettings.holdWalkKey = value;
        }

        private void ApplyAndGoBack()
        {
            Apply();
            GoBack();
        }

        private void Apply()
        {
            if (settingsDirtyFlag == false)
                return;

            gameInputSettingsRepository.GetSettings()
                .Do(settings =>
                {
                    settings.holdDuckKey = mutableSettings.holdDuckKey;
                    settings.holdWalkKey = mutableSettings.holdWalkKey;

                    settingsDirtyFlag = false;
                })
                .ContinueWith(_ => gameInputSettingsRepository.SaveSettings())
                .Subscribe();
        }

        private void GoBack()
        {
            optionsScreenView.FocusMainScreen();
        }
    }
}