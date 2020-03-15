using SJ.Coroutines;
using SJ.Profiles;
using SJ.Profiles.Exceptions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SJ.UI
{
    public class MainMenu : SJMonoBehaviour
    {
        [SerializeField]
        private Button newGame, loadGame, resumeGame, options, exitDesktop, exitMainMenu, continueLastProfile;

        protected override void SJAwake()
        {
            //exit desktop button
            exitDesktop.onClick.AddListener(ExitToDesktop);

            //exit main menu button
            exitMainMenu.onClick.AddListener(ExitToMainMenu);

            //resume game button
            resumeGame.onClick.AddListener(Hide);

            //continue button
            continueLastProfile.onClick.AddListener(Continue);

        }

        protected override void SJOnEnable()
        {
            UpdateButtonStates();
        }

        private void ExitToDesktop()
        {
            var message = Application.TranslatorService.GetLineByTagOfCurrentLanguage("confirmation_menu_message_exit").FirstLetterToUpper();
            ConfirmationPopupProvider.ShowWith(message, UnityEngine.Application.Quit, () => { });
        }

        private void ExitToMainMenu()
        {
            var message = Application.TranslatorService.GetLineByTagOfCurrentLanguage("confirmation_menu_message_exit").FirstLetterToUpper();
            ConfirmationPopupProvider.ShowWith(message, GoMenu, () => { });
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void GoMenu()
        {
            Application.GameManager.EndSession();
        }

        private void Continue()
        {
            IProfileRepository profileRepository = Repositories.GetProfileRepository();
            ICoroutineScheduler coroutineScheduler = Application.CoroutineScheduler;
            IGameSettingsRepository gameSettingsRepository = Repositories.GetGameSettingsRepository();

            gameSettingsRepository.GetSettings()
                .Subscribe(gameSettings =>
                {
                    profileRepository.Exists(gameSettings.lastProfile)
                        .SelectMany(exists =>
                        {
                            if (exists)
                                return profileRepository.GetProfileDataFrom(gameSettings.lastProfile);
                            else
                                throw new NonExistentProfileException();
                        })
                        .Subscribe(
                            profileData => Application.GameManager.BeginSessionWithProfile(profileData),
                            error => Logger.LogConsole("Profile " + gameSettings.lastProfile + " missing")
                        );
                });
        }

        private void UpdateButtonStates()
        {
            if (Application.GameManager.IsInGame)
            {
                exitDesktop.gameObject.SetActive(true);
                exitMainMenu.gameObject.SetActive(true);
                newGame.gameObject.SetActive(false);
                loadGame.gameObject.SetActive(true);
                resumeGame.gameObject.SetActive(true);
                options.gameObject.SetActive(true);
                continueLastProfile.gameObject.SetActive(false);
            }
            else
            {
                Repositories.GetGameSettingsRepository().GetSettings()
                    .Subscribe(gameSettings =>
                    {
                        if (string.IsNullOrEmpty(gameSettings.lastProfile))
                            continueLastProfile.gameObject.SetActive(false);
                        else
                            continueLastProfile.gameObject.SetActive(true);

                        exitDesktop.gameObject.SetActive(true);
                        exitMainMenu.gameObject.SetActive(false);
                        newGame.gameObject.SetActive(true);
                        loadGame.gameObject.SetActive(true);
                        resumeGame.gameObject.SetActive(false);
                        options.gameObject.SetActive(true);
                    });
            }
        }
    }

}

