using SJ.Management;
using UniRx;

namespace SJ.UI
{
    public class MainScreenViewController
    {
        private IMainScreenView view;
        private IGameManager gameManager;
        private IGameSettingsRepository gameSettingsRepository;
        private IMainMenu mainMenu;

        public MainScreenViewController(IMainScreenView view, IGameManager gameManager, IGameSettingsRepository gameSettingsRepository, IMainMenu mainMenu)
        {
            this.view = view;
            this.gameManager = gameManager;
            this.gameSettingsRepository = gameSettingsRepository;
            this.mainMenu = mainMenu;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            view.OnAppeared += ShowButtonsDependingOnApplicationState;

            view.OnNewGameClicked += mainMenu.FocusNewGameScreen;
            view.OnLoadGameClicked += mainMenu.FocusLoadGameScreen;
            view.OnOptionsClicked += mainMenu.FocusOptionsScreen;
            view.OnResumeGameClicked += mainMenu.Hide;
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
    }
}