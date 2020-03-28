using UnityEngine.Events;
using System;

namespace SJ.Menu
{
    public interface IGameInputSettingsScreenView
    {
        event UnityAction<bool> OnHoldDuckKeyToggleChanged;
        event UnityAction<bool> OnHoldWalkKeyToggleChanged;
        event UnityAction OnGoToKeyboardMappingClicked;
        event UnityAction OnGoToJoystickMappingClicked;
        event UnityAction OnSaveButtonClicked;
        event UnityAction OnBackButtonClicked;
        event Action OnAppeared;

        void FocusKeyboardMappingScreen();
        void FocusJoystickMappingScreen();
        void FocusMainScreen();

        void SetHoldDuckKeyToggleValue(bool value);
        void SetHoldWalkKeyToggleValue(bool value);

        void ShowApplyConfirmationPopup(string message, Action onAccept, Action onCancel);
    }
}