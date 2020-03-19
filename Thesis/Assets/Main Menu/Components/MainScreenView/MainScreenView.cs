using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class MainScreenView : SJMonoBehaviour, IMainScreenView
    {
        [SerializeField]
        private Button newGameButton, loadGameButton, continueButton, resumeGameButton, optionsButton, exitToDesktopButton, exitToMainMenuButton;

        public event UnityAction OnNewGameClicked
        {
            add { newGameButton.onClick.AddListener(value); }
            remove { newGameButton.onClick.RemoveListener(value); }
        }

        public event UnityAction OnLoadGameClicked
        {
            add { loadGameButton.onClick.AddListener(value); }
            remove { loadGameButton.onClick.RemoveListener(value); }
        }

        public event UnityAction OnContinueClicked
        {
            add { continueButton.onClick.AddListener(value); }
            remove { continueButton.onClick.RemoveListener(value); }
        }

        public event UnityAction OnResumeGameClicked
        {
            add { resumeGameButton.onClick.AddListener(value); }
            remove { resumeGameButton.onClick.RemoveListener(value); }
        }

        public event UnityAction OnOptionsClicked
        {
            add { optionsButton.onClick.AddListener(value); }
            remove { optionsButton.onClick.RemoveListener(value); }
        }

        public event UnityAction OnExitToDesktopClicked
        {
            add { exitToDesktopButton.onClick.AddListener(value); }
            remove { exitToDesktopButton.onClick.RemoveListener(value); }
        }

        public event UnityAction OnExitToMainMenuClicked
        {
            add { exitToMainMenuButton.onClick.AddListener(value); }
            remove { exitToMainMenuButton.onClick.RemoveListener(value); }
        }

        public event Action OnAppeared;

        protected override void SJOnEnable()
        {
            OnAppeared?.Invoke();
        }

        public void ShowInGameButtons()
        {
            exitToDesktopButton.gameObject.SetActive(true);
            exitToMainMenuButton.gameObject.SetActive(true);
            newGameButton.gameObject.SetActive(false);
            loadGameButton.gameObject.SetActive(true);
            resumeGameButton.gameObject.SetActive(true);
            optionsButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(false);
        }

        public void ShowInMenuButtons()
        {
            exitToDesktopButton.gameObject.SetActive(true);
            exitToMainMenuButton.gameObject.SetActive(false);
            newGameButton.gameObject.SetActive(true);
            loadGameButton.gameObject.SetActive(true);
            resumeGameButton.gameObject.SetActive(false);
            optionsButton.gameObject.SetActive(true);
        }

        public void ShowContinueButton()
        {
            continueButton.gameObject.SetActive(true);
        }

        public void HideContinueButton()
        {
            continueButton.gameObject.SetActive(false);
        }
    }
}