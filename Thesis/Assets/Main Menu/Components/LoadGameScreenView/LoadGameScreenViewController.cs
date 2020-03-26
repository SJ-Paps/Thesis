using SJ.Management;
using UniRx;

namespace SJ.Menu
{
    public class LoadGameScreenViewController
    {
        private ILoadGameScreenView view;
        private IProfileRepository profileRepository;
        private IGameSettingsRepository gameSettingsRepository;
        private IGameManager gameManager;

        public LoadGameScreenViewController(ILoadGameScreenView view, IProfileRepository profileRepository,
            IGameSettingsRepository gameSettingsRepository, IGameManager gameManager)
        {
            this.view = view;
            this.profileRepository = profileRepository;
            this.gameSettingsRepository = gameSettingsRepository;
            this.gameManager = gameManager;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            view.OnAppeared += UpdateProfiles;
            view.OnProfileSelectClicked += BeginSessionFor;
            view.OnProfileDeleteClicked += DeleteProfile;

            UpdateProfiles();
        }

        private void UpdateProfiles()
        {
            profileRepository.GetAllProfiles()
                .Subscribe(profiles => view.ShowProfiles(profiles));
        }

        private void BeginSessionFor(string profile)
        {
            gameSettingsRepository.GetSettings()
                .Do(gameSettings => gameSettings.lastProfile = profile)
                .ContinueWith(_ => gameSettingsRepository.SaveSettings())
                .Do(_ => gameManager.BeginSessionFor(profile))
                .Subscribe();
        }

        private void DeleteProfile(string profile)
        {
            gameSettingsRepository.GetSettings()
                .Do(gameSettings => ClearLastProfileIfMatches(profile, gameSettings))
                .ContinueWith(_ => profileRepository.DeleteProfile(profile))
                .Do(_ => UpdateProfiles())
                .Subscribe();
        }

        private static void ClearLastProfileIfMatches(string profile, GameSettings gameSettings)
        {
            if (gameSettings.lastProfile == profile)
                gameSettings.lastProfile = string.Empty;
        }
    }
}