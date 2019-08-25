using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileInfoItem : MonoBehaviour
{
    [SerializeField]
    private Text profileNameText;

    [SerializeField]
    private Button selectButton, deleteButton;

    public event Action<ProfileInfoItem> onSelectRequest;
    public event Action<ProfileInfoItem> onDeleteRequest;

    public ProfileData ProfileData { get; private set; }

    void Start()
    {
        selectButton.onClick.AddListener(OnSelectRequest);
        deleteButton.onClick.AddListener(OnDeleteRequest);
    }

    public void SetInfo(ProfileData profileData)
    {
        ProfileData = profileData;

        profileNameText.text = ProfileData.name;
    }

    private void OnSelectRequest()
    {
        if(onSelectRequest != null)
        {
            onSelectRequest(this);
        }
    }

    private void OnDeleteRequest()
    {
        if(onDeleteRequest != null)
        {
            onDeleteRequest(this);
        }
    }
}
