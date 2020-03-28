using SJ.Management.Localization;
using SJ.Management;
using UniRx;

namespace SJ.Menu
{
    public class MainScreenViewController
    {
        private const string ExitConfirmationMessageTag = "confirmation_menu_message_exit";

        private IMainScreenView view;
        private IGameManager gameManager;
        private IGameSettingsRepository gameSettingsRepository;
        private IMainMenu mainMenu;
        private ITranslatorService translatorService;

        public MainScreenViewController(IMainScreenView view, IGameManager gameManager, IGameSettingsRepository gameSettingsRepository, IMainMenu mainMenu, ITranslatorService translatorService)
        {
            this.view = view;
            this.gameManager = gameManager;
            this.gameSettingsRepository = gameSettingsRepository;
            this.mainMenu = mainMenu;
            this.translatorService = translatorService;

            Initialize();
        }

        private void Initialize()
        {
            view.OnAppeared += ShowButtonsDependingOnApplicationState;

            view.OnNewGameClicked += mainMenu.FocusNewGameScreen;
            view.OnLoadGameClicked += mainMenu.FocusLoadGameScreen;
            view.OnOptionsClicked += mainMenu.FocusOptionsScreen;
            view.OnResumeGameClicked += mainMenu.Hide;
            view.OnContinueClicked += Continue;
            view.OnExitToMainMenuClicked += ExitToMainMenu;
            view.OnExitToDesktopClicked += ExitToDesktop;
        }

        private void ShowButtonsDependingOnApplicationState()
        {
            if (gameManager.IsInGame())
            {
                view.ShowInGameButtons();
                view.HideContinueButton();
            }
            else
            {
                view.ShowInMenuButtons();

                string lastProfile = gameSettingsRepository.GetSettings().Wait().lastProfile;

                if (string.IsNullOrEmpty(lastProfile))
                    view.HideContinueButton();
                else
                    view.ShowContinueButton();
            }
        }

        private void Continue()
        {
            gameSettingsRepository.GetSettings()
                .Where(gameSettings => string.IsNullOrEmpty(gameSettings.lastProfile) == false)
                .Do(gameSettings => gameManager.BeginSessionFor(gameSettings.lastProfile))
                .Subscribe();
        }

        private void ExitToDesktop()
        {
            var message = translatorService.GetLineByTagOfCurrentLanguage(ExitConfirmationMessageTag).ToTitleCase();
            mainMenu.ShowConfirmationPopup(message, UnityEngine.Application.Quit, () => { });
        }

        private void ExitToMainMenu()
        {
            var message = translatorService.GetLineByTagOfCurrentLanguage(ExitConfirmationMessageTag).ToTitleCase();
            mainMenu.ShowConfirmationPopup(message, GoMenu, () => { });
        }

        private void GoMenu()
        {
            gameManager.EndSession();
        }
    }
}