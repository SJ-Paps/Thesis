using UnityEngine.Events;
using System;

namespace SJ.Menu
{
    public interface IGeneralGameInputSettingsScreenView
    {
        event UnityAction<bool> OnHoldDuckKeyToggleChanged;
        event UnityAction<bool> OnHoldWalkKeyToggleChanged;
        event UnityAction OnGoToKeyboardMappingClicked;
        event UnityAction OnGoToJoystickMapClicked;
        event UnityAction OnSaveButtonClicked;
        event UnityAction OnBackButtonClicked;
        event Action OnAppeared;

        void FocusKeyboardMappingScreen();
        void FocusJoystickMapScreen();
        void FocusMainScreen();

        void SetHoldDuckKeyToggleValue(bool value);
        void SetHoldWalkKeyToggleValue(bool value);

        void ShowApplyConfirmationPopup(string message, Action onAccept, Action onCancel);
    }
}