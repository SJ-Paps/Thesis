using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SJ.Profiles;
using SJ.Game;

namespace SJ.UI
{
    public class ProfileMenu : SJMonoBehaviour
    {
        [SerializeField]
        private ProfileInfoItem profileInfoItemPrefab;

        [SerializeField]
        private Transform layoutObject;

        private List<ProfileInfoItem> items;

        private IProfileRepository profileRepository;
        private IGameSettingsRepository gameSettingsRepository;

        protected override void SJAwake()
        {
            profileRepository = Repositories.GetProfileRepository();
            gameSettingsRepository = Repositories.GetGameSettingsRepository();
            items = new List<ProfileInfoItem>();
        }

        protected override void SJOnEnable()
        {
            Application.GetCoroutineScheduler().StartCoroutine(WaitLoadProfiles());
        }

        protected override void SJOnDisable()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
                items.RemoveAt(i);
                i--;
            }
        }

        private IEnumerator WaitLoadProfiles()
        {
            Task<ProfileData[]> taskProfiles = profileRepository.GetAllProfileData();

            while (taskProfiles.IsCompleted == false)
            {
                yield return null;
            }

            ProfileData[] profiles = taskProfiles.Result;

            if (profiles != null && profiles.Length > 0)
            {
                for (int i = 0; i < profiles.Length; i++)
                {
                    ProfileInfoItem instance = Instantiate(profileInfoItemPrefab, layoutObject);
                    items.Add(instance);
                    instance.SetInfo(profiles[i]);
                    instance.onSelectRequest += OnSelectProfile;
                    instance.onDeleteRequest += OnDeleteProfile;
                }
            }

        }

        private void OnSelectProfile(ProfileInfoItem item)
        {
            gameSettingsRepository.GetSettingsSynchronously().lastProfile = item.ProfileData.name;

            gameSettingsRepository.SaveSettingsSynchronously();

            GameManager.GetInstance().BeginSessionWithProfile(item.ProfileData);
        }

        private void OnDeleteProfile(ProfileInfoItem item)
        {
            GameSettings gameSettings = gameSettingsRepository.GetSettingsSynchronously();

            if (gameSettings.lastProfile == item.ProfileData.name)
            {
                gameSettings.lastProfile = null;

                gameSettingsRepository.SaveSettingsSynchronously();
            }

            profileRepository.DeleteProfile(item.ProfileData.name).Wait();
            items.Remove(item);
            Destroy(item.gameObject);
        }

    }
}


