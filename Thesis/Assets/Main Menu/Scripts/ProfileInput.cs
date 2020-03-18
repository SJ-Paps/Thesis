using UnityEngine;
using UnityEngine.UI;
using SJ.Profiles;
using SJ.Coroutines;
using SJ.Management;
using UniRx;

namespace SJ.UI
{
    public class ProfileInput : SJMonoBehaviour
    {
        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Button submitButton;

        [SerializeField]
        private Text notificationText;

        private IProfileRepository profileRepository;
        private IGameSettingsRepository gameSettingsRepository;

        protected override void SJAwake()
        {
            profileRepository = Repositories.GetProfileRepository();
            gameSettingsRepository = Repositories.GetGameSettingsRepository();
        }

        protected override void SJStart()
        {
            submitButton.onClick.AddListener(OnSubmit);
        }

        protected override void SJOnEnable()
        {
            notificationText.gameObject.SetActive(false);
        }

        private void OnSubmit()
        {
            string profileName = inputField.text;

            if(IsValidProfileName(profileName))
            {
                profileRepository.Exists(profileName)
                    .Subscribe(exists =>
                    {
                        if (exists)
                        {
                            inputField.text = string.Empty;
                            notificationText.gameObject.SetActive(true);
                            notificationText.text = Application.TranslatorService.GetLineByTagOfCurrentLanguage("notification_profile_in_use").FirstLetterToUpper();
                        }
                        else
                        {
                            gameSettingsRepository.GetSettings()
                                .SelectMany(gameSettings =>
                                {
                                    gameSettings.lastProfile = profileName;

                                    return gameSettingsRepository.SaveSettings();
                                })
                                .Subscribe(_ => Application.GameManager.BeginSessionFor(profileName));
                        }
                    });
            }
        }

        private bool IsValidProfileName(string profileName)
        {
            return string.IsNullOrEmpty(profileName) == false;
        }
    }

}

