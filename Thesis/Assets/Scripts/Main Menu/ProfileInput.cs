﻿using UnityEngine;
using UnityEngine.UI;
using SJ.Profiles;
using SJ.Coroutines;
using SJ.Game;
using UniRx;

namespace SJ.UI
{
    public class ProfileInput : SJMonoBehaviour
    {
        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Button submitButton;

        [SerializeField]
        private Text notificationText;

        private IProfileRepository profileRepository;
        private IGameSettingsRepository gameSettingsRepository;

        protected override void SJAwake()
        {
            profileRepository = Repositories.GetProfileRepository();
            gameSettingsRepository = Repositories.GetGameSettingsRepository();
        }

        protected override void SJStart()
        {
            submitButton.onClick.AddListener(OnSubmit);
        }

        protected override void SJOnEnable()
        {
            notificationText.gameObject.SetActive(false);
        }

        private void OnSubmit()
        {
            string profileName = inputField.text;

            if(IsValidProfileName(profileName))
            {
                Application.GetCoroutineScheduler()
                .AwaitTask(profileRepository.Exists(profileName),
                delegate (bool exists)
                {
                    if (exists)
                    {
                        inputField.text = string.Empty;
                        notificationText.gameObject.SetActive(true);
                        notificationText.text = Application.GetTranslatorService().GetLineByTagOfCurrentLanguage("notification_profile_in_use").FirstLetterToUpper();
                    }
                    else
                    {
                        gameSettingsRepository.GetSettings()
                            .SelectMany(gameSettings => 
                            {
                                gameSettings.lastProfile = profileName;

                                return gameSettingsRepository.SaveSettings();
                            })
                            .Subscribe(_ => GameManager.GetInstance().BeginSessionWithProfile(new ProfileData() { name = profileName }));
                    }
                });
            }
        }

        private bool IsValidProfileName(string profileName)
        {
            return string.IsNullOrEmpty(profileName) == false;
        }
    }

}

