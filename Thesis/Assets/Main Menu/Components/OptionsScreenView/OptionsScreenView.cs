using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class OptionsScreenView : SJMonoBehaviour, IOptionsScreenView
    {
        [SerializeField]
        private Button goToSoundSettingsButton, goToLanguageSettingsButton, goToGameInputSettingsButton, backButton;

        [SerializeField]
        private GameObject mainScreen, soundSettingsScreen, languageSettingsScreen, gameInputSettingsScreen;

        private GameObject[] screens;

        public event UnityAction OnGoToSoundsSettingsButtonClicked
        {
            add { goToSoundSettingsButton.onClick.AddListener(value); }
            remove { goToSoundSettingsButton.onClick.RemoveListener(value); }
        }
        public event UnityAction OnGoToLanguageSettingsButtonClicked
        {
            add { goToLanguageSettingsButton.onClick.AddListener(value); }
            remove { goToLanguageSettingsButton.onClick.RemoveListener(value); }
        }
        public event UnityAction OnGoToGameInputSettingsButtonClicked
        {
            add { goToGameInputSettingsButton.onClick.AddListener(value); }
            remove { goToGameInputSettingsButton.onClick.RemoveListener(value); }
        }
        public event UnityAction OnBackButtonClicked
        {
            add { backButton.onClick.AddListener(value); }
            remove { backButton.onClick.RemoveListener(value); }
        }

        protected override void SJAwake()
        {
            screens = new GameObject[4];

            screens[0] = soundSettingsScreen;
            screens[1] = languageSettingsScreen;
            screens[2] = gameInputSettingsScreen;
            screens[3] = mainScreen;
        }

        public void FocusGameInputSettingsScreen() => FocusScreen(gameInputSettingsScreen);

        public void FocusLanguageSettingsScreen() => FocusScreen(languageSettingsScreen);

        public void FocusSoundSettingsScreen() => FocusScreen(soundSettingsScreen);

        public void FocusMainScreen() => FocusScreen(mainScreen);

        private void FocusScreen(GameObject screen)
        {
            screen.SetActive(true);

            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i] != screen)
                    screens[i].SetActive(false);
            }
        }
    }
}