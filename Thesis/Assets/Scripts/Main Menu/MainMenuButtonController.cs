using UnityEngine;
using UnityEngine.UI;
using SJ.Profiles;
using SJ.Profiles.Exceptions;
using System.Threading.Tasks;
using SJ.Coroutines;
using System;
using SJ.Game;
using UniRx;

namespace SJ.UI
{
    public class MainMenuButtonController : SJMonoBehaviour
    {

        [SerializeField]
        private Button newGame, loadGame, resumeGame, options, exitDesktop, exitMainMenu, continueLastProfile;

        protected override void SJAwake()
        {
            MainMenu.GetInstance().onShow += UpdateButtonStates;

            //exit desktop button
            exitDesktop.onClick.AddListener(ExitToDesktop);

            //exit main menu button
            exitMainMenu.onClick.AddListener(ExitToMainMenu);

            //resume game button
            resumeGame.onClick.AddListener(HideMenu);

            //continue button
            continueLastProfile.onClick.AddListener(Continue);

        }

        protected override void SJOnEnable()
        {
            UpdateButtonStates();
        }

        private void ExitToDesktop()
        {
            MainMenu.GetInstance().DisplayConfirmationMenu(Application.GetTranslatorService().GetLineByTagOfCurrentLanguage("confirmation_menu_message_exit").FirstLetterToUpper(), UnityEngine.Application.Quit, null);
        }

        private void ExitToMainMenu()
        {
            MainMenu.GetInstance().DisplayConfirmationMenu(Application.GetTranslatorService().GetLineByTagOfCurrentLanguage("confirmation_menu_message_exit").FirstLetterToUpper(), GoMenu, null);
        }

        private void HideMenu()
        {
            MainMenu.GetInstance().Hide();
        }

        private void GoMenu()
        {
            GameManager.GetInstance().EndSession();
        }

        private void Continue()
        {
            IProfileRepository profileRepository = Repositories.GetProfileRepository();
            ICoroutineScheduler coroutineScheduler = Application.GetCoroutineScheduler();
            IGameSettingsRepository gameSettingsRepository = Repositories.GetGameSettingsRepository();

            gameSettingsRepository.GetSettings()
                .Subscribe(gameSettings =>
                {
                    profileRepository.Exists(gameSettings.lastProfile)
                        .SelectMany(exists =>
                        {
                            if (exists)
                                return profileRepository.GetProfileDataFrom(gameSettings.lastProfile);
                            else
                                throw new NonExistentProfileException();
                        })
                        .Subscribe(
                            profileData => GameManager.GetInstance().BeginSessionWithProfile(profileData),
                            error => Logger.LogConsole("Profile " + gameSettings.lastProfile + " missing")
                        );
                });
        }

        private void UpdateButtonStates()
        {
            if (GameManager.GetInstance().IsInGame)
            {
                exitDesktop.gameObject.SetActive(true);
                exitMainMenu.gameObject.SetActive(true);
                newGame.gameObject.SetActive(false);
                loadGame.gameObject.SetActive(true);
                resumeGame.gameObject.SetActive(true);
                options.gameObject.SetActive(true);
                continueLastProfile.gameObject.SetActive(false);
            }
            else
            {
                Repositories.GetGameSettingsRepository().GetSettings()
                    .Subscribe(gameSettings =>
                    {
                        if (string.IsNullOrEmpty(gameSettings.lastProfile))
                            continueLastProfile.gameObject.SetActive(false);
                        else
                            continueLastProfile.gameObject.SetActive(true);

                        exitDesktop.gameObject.SetActive(true);
                        exitMainMenu.gameObject.SetActive(false);
                        newGame.gameObject.SetActive(true);
                        loadGame.gameObject.SetActive(true);
                        resumeGame.gameObject.SetActive(false);
                        options.gameObject.SetActive(true);
                    });
            }
        }
    }

}

