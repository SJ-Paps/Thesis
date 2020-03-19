using System;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Menu
{
    public class LoadGameScreenView : SJMonoBehaviour, ILoadGameScreenView
    {
        [SerializeField]
        private ProfileInfoItem profileInfoItemPrefab;

        [SerializeField]
        private Transform profileInfoItemLayout;

        private List<ProfileInfoItem> items = new List<ProfileInfoItem>();

        public event Action OnAppeared;
        public event Action<string> OnProfileSelectClicked;
        public event Action<string> OnProfileDeleteClicked;

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