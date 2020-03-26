using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class ProfileInfoItem : SJMonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI profileNameText;

        [SerializeField]
        private Button selectButton, deleteButton;

        public event Action<string> OnSelectClicked;
        public event Action<string> OnDeleteClicked;

        protected override void SJStart()
        {
            selectButton.onClick.AddListener(() => OnSelectClicked?.Invoke(profileNameText.text));
            deleteButton.onClick.AddListener(() => OnDeleteClicked?.Invoke(profileNameText.text));
        }

        public void SetProfile(string profile)
        {
            profileNameText.text = profile;
        }
    }
}