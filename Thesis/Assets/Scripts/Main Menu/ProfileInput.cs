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

    private void OnSubmit()
    {
        string profileName = inputField.text;

        if(ProfileCareTaker.ProfileExistsOrIsValid(profileName))
        {
            inputField.text = string.Empty;
            notificationText.text = "Profile is in use. Choose another name.";
        }
        else
        {
            GameManager.GetInstance().BeginSessionWithProfile(new ProfileData() { name = profileName });
        }
    }
}
