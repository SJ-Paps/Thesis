using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

namespace SJ.Menu
{
    public class GeneralGameInputSettingsScreenView : SJMonoBehaviour, IGeneralGameInputSettingsScreenView
    {
        [SerializeField]
        private Toggle holdDuckKeyToggle, holdWalkKeyToggle;

        [SerializeField]
        private Button goToKeyboardMappingButton, goToJoystickMapButton, saveButton, backButton;

        [SerializeField]
        private GameObject mainScreen, keyboardMappingScreen, joystickMapScreen;

        public event UnityAction<bool> OnHoldDuckKeyToggleChanged
        {
            add { holdDuckKeyToggle.onValueChanged.AddListener(value); }
            remove { holdDuckKeyToggle.onValueChanged.RemoveListener(value); }
        }
        public event UnityAction<bool> OnHoldWalkKeyToggleChanged
        {
            add { holdWalkKeyToggle.onValueChanged.AddListener(value); }
            remove { holdWalkKeyToggle.onValueChanged.RemoveListener(value); }
        }
        public event UnityAction OnGoToKeyboardMappingClicked
        {
            add { goToKeyboardMappingButton.onClick.AddListener(value); }
            remove { goToKeyboardMappingButton.onClick.RemoveListener(value); }
        }
        public event UnityAction OnGoToJoystickMapClicked
        {
            add { goToJoystickMapButton.onClick.AddListener(value); }
            remove { goToJoystickMapButton.onClick.RemoveListener(value); }
        }
        public event UnityAction OnSaveButtonClicked
        {
            add { saveButton.onClick.AddListener(value); }
            remove { saveButton.onClick.RemoveListener(value); }
        }
        public event UnityAction OnBackButtonClicked
        {
            add { backButton.onClick.AddListener(value); }
            remove { backButton.onClick.RemoveListener(value); }
        }

        public event Action OnAppeared;

        private void OnEnable()
        {
            OnAppeared?.Invoke();
        }

        public void ShowApplyConfirmationPopup(string message, Action onAccept, Action onCancel)
        {
            ConfirmationPopupProvider.ShowWith(message, onAccept, onCancel);
        }

        public void FocusJoystickMapScreen()
        {
            joystickMapScreen.SetActive(true);
            mainScreen.SetActive(false);
        }

        public void FocusKeyboardMappingScreen()
        {
            keyboardMappingScreen.SetActive(true);
            mainScreen.SetActive(false);
        }

        public void FocusMainScreen()
        {
            mainScreen.SetActive(true);
            joystickMapScreen.SetActive(false);
            keyboardMappingScreen.SetActive(false);
        }

        public void SetHoldDuckKeyToggleValue(bool value)
        {
            holdDuckKeyToggle.SetIsOnWithoutNotify(value);
        }

        public void SetHoldWalkKeyToggleValue(bool value)
        {
            holdWalkKeyToggle.SetIsOnWithoutNotify(value);
        }
    }
}