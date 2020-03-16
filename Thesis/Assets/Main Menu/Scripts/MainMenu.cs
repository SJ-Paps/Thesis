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

        [SerializeField]
        private GameObject firstMenu, optionsMenu, newGameMenu, loadGameMenu;

        protected override void SJAwake()
        {
            exitDesktop.onClick.AddListener(ExitToDesktop);
            exitMainMenu.onClick.AddListener(ExitToMainMenu);
            resumeGame.onClick.AddListener(Hide);
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
                .Where(gameSettings => string.IsNullOrEmpty(gameSettings.lastProfile) == false)
                .Do(gameSettings => Application.GameManager.BeginSessionFor(gameSettings.lastProfile))
                .Subscribe();
        }

        private void UpdateButtonStates()
        {
            if (Application.GameManager.IsInGame())
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

