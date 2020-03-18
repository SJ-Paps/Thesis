using UnityEngine;

namespace SJ.UI
{
    public class MainMenu : SJMonoBehaviour, IMainMenu
    {
        [SerializeField]
        private GameObject mainScreen, optionsScreen, newGameScreen, loadGameScreen;

        private GameObject[] screens;

        protected override void SJAwake()
        {
            screens = new GameObject[4];

            screens[0] = mainScreen;
            screens[1] = optionsScreen;
            screens[2] = newGameScreen;
            screens[3] = loadGameScreen;
        }

        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);

        public void FocusMainScreen() => FocusScreen(mainScreen);

        public void FocusOptionsScreen() => FocusScreen(optionsScreen);

        public void FocusNewGameScreen() => FocusScreen(newGameScreen);

        public void FocusLoadGameScreen() => FocusScreen(loadGameScreen);

        private void FocusScreen(GameObject screen)
        {
            screen.SetActive(true);

            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i] != screen)
                    screens[i].SetActive(false);
            }
        }

        /*protected override void SJOnEnable()
        {

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
        }*/
    }
}