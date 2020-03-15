using System;
using UnityEngine;
using UnityEngine.UI;
using SJ.Profiles;

namespace SJ.UI
{
    public class ProfileInfoItem : SJMonoBehaviour
    {
        [SerializeField]
        private Text profileNameText;

        [SerializeField]
        private Button selectButton, deleteButton;

        public event Action<ProfileInfoItem> onSelectRequest;
        public event Action<ProfileInfoItem> onDeleteRequest;

        public ProfileData ProfileData { get; private set; }

        protected override void SJStart()
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
            if (onSelectRequest != null)
            {
                onSelectRequest(this);
            }
        }

        private void OnDeleteRequest()
        {
            if (onDeleteRequest != null)
            {
                onDeleteRequest(this);
            }
        }
    }
}