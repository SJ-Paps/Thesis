using System;
using UnityEngine;
using UnityEngine.Events;

namespace SJ.Menu
{
    public interface IKeyboardMappingScreenView
    {
        event Action<string> OnRequestedMainKeyRebind;
        event Action<string> OnRequestedAlternativeKeyRebind;
        event Action<KeyCode> OnKeyListened;
        event UnityAction OnResetButtonClicked;
        event UnityAction OnSaveButtonClicked;
        event UnityAction OnBackButtonClicked;
        event Action OnAppeared;

        void CreateInputKeyGroupControl(string name, string displayName, KeyCode main, KeyCode alternative, string mainDisplayName, string alternativeDisplayName);
        void UpdateMainKeyOf(string name, KeyCode main, string displayName);
        void UpdateAlternativeKeyOf(string name, KeyCode alternative, string displayName);
        void ShowAsModifiableMainOf(string name);
        void ShowAsModifiableAlternativeOf(string name);
        void ShowAllAsNotModifiable();
        void ListenForInputs();
        void StopListeningInputs();
        void ShowConfirmationPopup(string message, Action onAccept, Action onCancel);
        void LockInteractableObjects();
        void UnlockInteractableObjects();
    }
}