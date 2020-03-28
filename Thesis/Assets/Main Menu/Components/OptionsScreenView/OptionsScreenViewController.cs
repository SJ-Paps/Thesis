namespace SJ.Menu
{
    public class OptionsScreenViewController
    {
        private IOptionsScreenView view;
        private IMainMenu mainMenu;

        public OptionsScreenViewController(IOptionsScreenView view, IMainMenu mainMenu)
        {
            this.view = view;
            this.mainMenu = mainMenu;

            Initialize();
        }

        private void Initialize()
        {
            view.OnGoToGameInputSettingsButtonClicked += view.FocusGameInputSettingsScreen;
            view.OnGoToLanguageSettingsButtonClicked += view.FocusLanguageSettingsScreen;
            view.OnGoToSoundsSettingsButtonClicked += view.FocusSoundSettingsScreen;
            view.OnBackButtonClicked += mainMenu.FocusMainScreen;
        }
    }
}