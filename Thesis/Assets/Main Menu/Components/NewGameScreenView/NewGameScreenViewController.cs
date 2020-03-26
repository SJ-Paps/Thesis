using SJ.Localization;
using SJ.Management;
using UniRx;

namespace SJ.Menu
{
    public class NewGameScreenViewController
    {
        private INewGameScreenView view;
        private IProfileRepository profileRepository;
        private IGameSettingsRepository gameSettingsRepository;
        private ITranslatorService translatorService;
        private IGameManager gameManager;

        public NewGameScreenViewController(INewGameScreenView view, IProfileRepository profileRepository, 
            IGameSettingsRepository gameSettingsRepository, ITranslatorService translatorService, IGameManager gameManager)
        {
            this.view = view;
            this.profileRepository = profileRepository;
            this.gameSettingsRepository = gameSettingsRepository;
            this.translatorService = translatorService;
            this.gameManager = gameManager;

            this.view.OnNewProfileSubmitted += ProcessProfileInput;
        }

        private void ProcessProfileInput(string profile)
        {
            view.HideErrorMessage();

            if (IsValidProfileName(profile) == false)
            {
                view.ShowErrorMessage(translatorService.GetLineByTagOfCurrentLanguage("notification_profile_invalid_name").FirstLetterToUpper());
                return;
            }
            else if (IsInUse(profile))
            {
                view.ShowErrorMessage(translatorService.GetLineByTagOfCurrentLanguage("notification_profile_in_use").FirstLetterToUpper());
                return;
            }

            SaveLastProfileAndBeginSession(profile);
        }

        private void SaveLastProfileAndBeginSession(string profile)
        {
            gameSettingsRepository.GetSettings()
                            .Do(gameSettings => gameSettings.lastProfile = profile)
                            .ContinueWith(_ => gameSettingsRepository.SaveSettings())
                            .Do(_ => gameManager.BeginSessionFor(profile))
                            .Subscribe();
        }

        private bool IsInUse(string profile)
        {
            return profileRepository.Exists(profile).Wait();
        }

        private bool IsValidProfileName(string profile)
        {
            return string.IsNullOrEmpty(profile) == false;
        }
    }
}
