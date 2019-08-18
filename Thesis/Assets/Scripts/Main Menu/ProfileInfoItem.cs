using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ProfileInfoItem : MonoBehaviour
{
    [SerializeField]
    private Text profileNameText;

    [SerializeField]
    private Button chooseButton;

    public event Action<ProfileInfoItem> onChoose;

    public ProfileData ProfileData { get; private set; }

    void Start()
    {
        chooseButton.onClick.AddListener(OnClick);
    }

    public void SetInfo(ProfileData profileData)
    {
        ProfileData = profileData;

        profileNameText.text = ProfileData.name;
    }

    private void OnClick()
    {
        if(onChoose != null)
        {
            onChoose(this);
        }
    }
}
