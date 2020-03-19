using System;
using UnityEngine;

namespace SJ.Menu
{
    public class MainMenu : SJMonoBehaviour, IMainMenu
    {
        [SerializeField]
        private GameObject mainScreen, optionsScreen, newGameScreen, loadGameScreen;

        private GameObject[] screens;

        protected override void SJAwake()
        {
            screens = new GameObject[4];

            screens[0] = mainScreen;
            screens[1] = optionsScreen;
            screens[2] = newGameScreen;
            screens[3] = loadGameScreen;
        }

        public void ShowConfirmationPopup(string message, Action onAccept, Action onCancel) => ConfirmationPopupProvider.ShowWith(message, onAccept, onCancel);

        public void Hide() => gameObject.SetActive(false);

        public void Show() => gameObject.SetActive(true);

        public void FocusMainScreen() => FocusScreen(mainScreen);

        public void FocusOptionsScreen() => FocusScreen(optionsScreen);

        public void FocusNewGameScreen() => FocusScreen(newGameScreen);

        public void FocusLoadGameScreen() => FocusScreen(loadGameScreen);

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