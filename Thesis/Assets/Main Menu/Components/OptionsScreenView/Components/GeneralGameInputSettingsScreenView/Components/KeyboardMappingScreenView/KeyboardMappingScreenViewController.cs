using SJ.Management;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SJ.Management.Localization;

namespace SJ.Menu
{
    public class KeyboardMappingScreenViewController
    {
        private class BackupInputKeyGroup
        {
            public string Name;
            public KeyCode Main;
            public KeyCode Alternative;

            public BackupInputKeyGroup(string name, KeyCode main, KeyCode alternative)
            {
                Name = name;
                Main = main;
                Alternative = alternative;
            }
        }

        private const string ActionNameLanguageTagPrefix = "input_action_";
        private const string ResetMessageLanguageTag = "confirmation_menu_message_reset_settings", SaveConfigurationMessageLanguageTag = "confirmation_menu_message_save_settings";

        private IKeyboardMappingScreenView view;
        private IGameInputSettingsRepository gameInputSettingsRepository;
        private ITranslatorService translatorService;
        private IGameInputSettingsScreenView gameInputSettingsScreenView;

        private string listeningGroup;
        private bool listeningMain;

        private bool dirtyFlag;

        private List<BackupInputKeyGroup> backupInputKeyGroups = new List<BackupInputKeyGroup>();

        public KeyboardMappingScreenViewController(IKeyboardMappingScreenView view, IGameInputSettingsRepository gameInputSettingsRepository,
            ITranslatorService translatorService, IGameInputSettingsScreenView gameInputSettingsScreenView)
        {
            this.view = view;
            this.gameInputSettingsRepository = gameInputSettingsRepository;
            this.translatorService = translatorService;
            this.gameInputSettingsScreenView = gameInputSettingsScreenView;

            Initialize();
        }

        private void Initialize()
        {
            view.OnRequestedMainKeyRebind += BeginListenForMainKeyRebindOf;
            view.OnRequestedAlternativeKeyRebind += BeginListenForAlternativeKeyRebindOf;

            view.OnKeyListened += ValidateInputKey;

            view.OnSaveButtonClicked += SaveConfiguration;
            view.OnAppeared += ResetAndRefresh;
            view.OnResetButtonClicked += ShowResetConfirmationIfSettingsHaveChanged;
            view.OnBackButtonClicked += ShowGoBackConfirmationIfSettingsHaveChangedOrGoBack;

            CreateInputKeyGroupControls();
        }

        private void CreateInputKeyGroupControls()
        {
            gameInputSettingsRepository.GetSettings()
                .Do(settings =>
                {
                    var inputKeyGroups = settings.GetGroups().Values;

                    foreach (var inputKeyGroup in inputKeyGroups)
                    {
                        view.CreateInputKeyGroupControl(inputKeyGroup.name, GetDisplayNameFor(inputKeyGroup.name), inputKeyGroup.main, inputKeyGroup.alternative,
                            GetDisplayNameFor(inputKeyGroup.main), GetDisplayNameFor(inputKeyGroup.alternative));

                        backupInputKeyGroups.Add(new BackupInputKeyGroup(inputKeyGroup.name, inputKeyGroup.main, inputKeyGroup.alternative));
                    }
                        
                })
                .Subscribe();
        }

        private string GetDisplayNameFor(KeyCode keyCode)
        {
            return keyCode.ToString();
        }

        private string GetDisplayNameFor(string groupName) 
            => translatorService.GetLineByTagOfCurrentLanguage((ActionNameLanguageTagPrefix + groupName).ToLower(), TextOptions.FirstLetterToUpper);

        private void BeginListenForMainKeyRebindOf(string groupName)
        {
            listeningGroup = groupName;
            listeningMain = true;

            view.ShowAsModifiableMainOf(groupName);
            view.LockInteractableObjects();

            view.ListenForInputs();
        }

        private void BeginListenForAlternativeKeyRebindOf(string groupName)
        {
            listeningGroup = groupName;
            listeningMain = false;

            view.ShowAsModifiableAlternativeOf(groupName);
            view.LockInteractableObjects();

            view.ListenForInputs();
        }

        private void ValidateInputKey(KeyCode keyCode)
        {
            if (IsCancelRebindKey(keyCode))
            {
                StopRebinding();
            }
            else if(IsRemoveKey(keyCode))
            {
                RebindKey(KeyCode.None);
                StopRebinding();
            }
            else if (IsInvalidKey(keyCode) == false)
            {
                RebindKey(keyCode);
                StopRebinding();
            }
        }

        private void StopRebinding()
        {
            view.ShowAllAsNotModifiable();
            view.StopListeningInputs();

            listeningGroup = string.Empty;
            listeningMain = false;

            view.UnlockInteractableObjects();
        }

        private bool IsCancelRebindKey(KeyCode keyCode) => keyCode.IsMouseKey();

        private bool IsInvalidKey(KeyCode keyCode) => keyCode.IsKeyboardKey() == false || keyCode == KeyCode.Escape;

        private bool IsRemoveKey(KeyCode keyCode) => keyCode == KeyCode.Backspace;

        private void RebindKey(KeyCode keyCode)
        {
            if (listeningMain)
            {
                BackupWithName(listeningGroup).Main = keyCode;
                view.UpdateMainKeyOf(listeningGroup, keyCode, GetDisplayNameFor(keyCode));
            }
            else
            {
                BackupWithName(listeningGroup).Alternative = keyCode;
                view.UpdateAlternativeKeyOf(listeningGroup, keyCode, GetDisplayNameFor(keyCode));
            }

            dirtyFlag = true;
        }

        private BackupInputKeyGroup BackupWithName(string name)
        {
            return backupInputKeyGroups.Find(backup => backup.Name == name);
        }

        private void Refresh()
        {
            for(int i = 0; i < backupInputKeyGroups.Count; i++)
            {
                var backup = backupInputKeyGroups[i];

                view.UpdateMainKeyOf(backup.Name, backup.Main, GetDisplayNameFor(backup.Main));
                view.UpdateAlternativeKeyOf(backup.Name, backup.Alternative, GetDisplayNameFor(backup.Alternative));
            }
        }

        private void ResetBackups()
        {
            dirtyFlag = false;

            gameInputSettingsRepository.GetSettings()
                .Do(settings =>
                {
                    var inputKeyGroups = settings.GetGroups().Values;

                    foreach(var inputKeyGroup in inputKeyGroups)
                    {
                        var backup = BackupWithName(inputKeyGroup.name);

                        backup.Main = inputKeyGroup.main;
                        backup.Alternative = inputKeyGroup.alternative;
                    }
                })
                .Subscribe();
        }

        private void ResetAndRefresh()
        {
            ResetBackups();
            Refresh();
        }

        private void SaveConfiguration()
        {
            gameInputSettingsRepository.GetSettings()
                .Do(settings =>
                {
                    for (int i = 0; i < backupInputKeyGroups.Count; i++)
                    {
                        var backup = backupInputKeyGroups[i];

                        settings.SetKeysTo(backup.Name, backup.Main, backup.Alternative);
                    }
                })
                .ContinueWith(_ => gameInputSettingsRepository.SaveSettings())
                .Do(_ => ResetAndRefresh())
                .Subscribe();
        }

        private void ShowResetConfirmationIfSettingsHaveChanged()
        {
            if (dirtyFlag)
                ShowResetConfirmationPopup();
        }

        private void ShowGoBackConfirmationIfSettingsHaveChangedOrGoBack()
        {
            if (dirtyFlag)
                ShowGoBackConfirmationPopup();
            else
                GoBack();
        }

        private void ShowResetConfirmationPopup()
        {
            view.ShowConfirmationPopup(translatorService.GetLineByTagOfCurrentLanguage(ResetMessageLanguageTag), ResetAndRefresh, () => { });
        }

        private void ShowGoBackConfirmationPopup()
        {
            view.ShowConfirmationPopup(translatorService.GetLineByTagOfCurrentLanguage(SaveConfigurationMessageLanguageTag), SaveConfigurationAndGoBack, ResetAndGoBack);
        }

        private void SaveConfigurationAndGoBack()
        {
            SaveConfiguration();
            GoBack();
        }

        private void ResetAndGoBack()
        {
            ResetAndRefresh();
            GoBack();
        }

        private void GoBack()
        {
            gameInputSettingsScreenView.FocusMainScreen();
        }
    }
}