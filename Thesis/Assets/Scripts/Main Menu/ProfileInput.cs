using UnityEngine;
using UnityEngine.UI;

public class ProfileInput : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Button submitButton;

    [SerializeField]
    private Text notificationText;

    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnEnable()
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
            GameManager.GetInstance().BeginSessionWithProfile(new ProfileData() { name = profileName });
        }
    }
}
