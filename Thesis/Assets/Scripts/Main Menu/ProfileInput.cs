using UnityEngine;
using UnityEngine.UI;
using SJ.Profiles;
using SJ.Coroutines;

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
                Application.GetCoroutineScheduler()
                .AwaitTask(Repositories.GetProfileRepository().Exists(profileName),
                delegate (bool exists)
                {
                    if (exists)
                    {
                        inputField.text = string.Empty;
                        notificationText.gameObject.SetActive(true);
                        notificationText.text = Application.GetTranslatorService().GetLineByTagOfCurrentLanguage("notification_profile_in_use").FirstLetterToUpper();
                    }
                    else
                    {
                        ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

                        gameConfiguration.lastProfile = profileName;

                        GameConfigurationCareTaker.SaveConfiguration();

                        GameManager.GetInstance().BeginSessionWithProfile(new ProfileData() { name = profileName });
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

