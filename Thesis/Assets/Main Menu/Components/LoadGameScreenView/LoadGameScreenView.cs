using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class LoadGameScreenView : SJMonoBehaviour, ILoadGameScreenView
    {
        [SerializeField]
        private ProfileInfoItem profileInfoItemPrefab;

        [SerializeField]
        private Transform profileInfoItemLayout;

        [SerializeField]
        private Button backButton;

        private List<ProfileInfoItem> items = new List<ProfileInfoItem>();

        public event Action OnAppeared;
        public event Action<string> OnProfileSelectClicked;
        public event Action<string> OnProfileDeleteClicked;

        public event UnityAction OnBackButtonClicked
        {
            add { backButton.onClick.AddListener(value); }
            remove { backButton.onClick.RemoveListener(value); }
        }

        protected override void SJOnEnable()
        {
            OnAppeared?.Invoke();
        }

        public void ShowProfiles(string[] profiles)
        {
            DestroyItems();

            foreach(var profile in profiles)
                CreateProfileItem(profile);
        }

        private void CreateProfileItem(string profile)
        {
            var profileItem = Instantiate(profileInfoItemPrefab, profileInfoItemLayout);
            profileItem.SetProfile(profile);
            profileItem.OnSelectClicked += selectedProfile => OnProfileSelectClicked?.Invoke(selectedProfile);
            profileItem.OnDeleteClicked += selectedProfile => OnProfileDeleteClicked?.Invoke(selectedProfile);
            items.Add(profileItem);
        }

        private void DestroyItems()
        {
            foreach (var item in items)
                Destroy(item.gameObject);

            items.Clear();
        }
    }
}