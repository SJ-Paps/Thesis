using UnityEngine;
using UnityEngine.UI;

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

        if(ProfileCareTaker.ProfileExistsAndIsValid(profileName))
        {
            inputField.text = string.Empty;
            notificationText.gameObject.SetActive(true);
            notificationText.text = LanguageManager.GetLocalizedTextLibrary().GetLineByTagOfCurrentLanguage("notification_profile_in_use").FirstLetterToUpper();
        }
        else
        {
            ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

            gameConfiguration.lastProfile = profileName;

            GameConfigurationCareTaker.SaveConfiguration();

            GameManager.GetInstance().BeginSessionWithProfile(new ProfileData() { name = profileName });
        }
    }
}
