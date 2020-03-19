using Paps.Maybe;
using SJ.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace SJ.Menu
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
            LoadProfiles();
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

        private void LoadProfiles()
        {
            profileRepository.GetAllProfiles()
                .SelectMany(profileNames => GetProfileDataFrom(profileNames))
                .Subscribe(profiles => profiles.ForEach(p => InstantiateProfileItem(p)));
        }

        private IObservable<List<ProfileData>> GetProfileDataFrom(string[] profiles)
        {
            return Observable.Create<List<ProfileData>>(observer =>
            {
                var profileDataList = new List<ProfileData>();
                var getProfileObservables = new List<IObservable<Maybe<ProfileData>>>(); 

                for (int i = 0; i < profiles.Length; i++)
                    getProfileObservables.Add(profileRepository.GetProfileDataFrom(profiles[i]));

                Observable.Zip(getProfileObservables)
                    .ObserveOnMainThread()
                    .Subscribe(profileDataMaybes =>
                    {
                        foreach(var maybe in profileDataMaybes)
                        {
                            if (maybe.IsSomething())
                                profileDataList.Add(maybe.Value);
                        }

                        observer.OnNext(profileDataList);
                        observer.OnCompleted();
                    });

                return Disposable.Empty;
            });
        }

        private void InstantiateProfileItem(ProfileData profileData)
        {
            ProfileInfoItem instance = Instantiate(profileInfoItemPrefab, layoutObject);
            items.Add(instance);
            instance.SetInfo(profileData);
            instance.onSelectRequest += OnSelectProfile;
            instance.onDeleteRequest += OnDeleteProfile;
        }

        private void OnSelectProfile(ProfileInfoItem item)
        {
            gameSettingsRepository.GetSettings()
                .Subscribe(gameSettings =>
                {
                    gameSettings.lastProfile = item.ProfileData.name;

                    gameSettingsRepository.SaveSettings().Subscribe();
                });

            Application.GameManager.BeginSessionFor(item.ProfileData.name);
        }

        private void OnDeleteProfile(ProfileInfoItem item)
        {
            gameSettingsRepository.GetSettings()
                .Subscribe(gameSettings =>
                {
                    profileRepository.DeleteProfile(item.ProfileData.name).Wait();
                    items.Remove(item);
                    Destroy(item.gameObject);

                    if (gameSettings.lastProfile == item.ProfileData.name)
                    {
                        gameSettings.lastProfile = null;

                        gameSettingsRepository.SaveSettings().Subscribe();
                    }
                });
        }

    }
}


