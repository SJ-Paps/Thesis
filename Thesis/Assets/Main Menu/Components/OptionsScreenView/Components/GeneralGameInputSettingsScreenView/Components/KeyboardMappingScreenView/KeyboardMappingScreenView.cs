using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SJ.Menu
{
    public class KeyboardMappingScreenView : SJMonoBehaviour, IKeyboardMappingScreenView
    {
        [SerializeField]
        private InputKeyGroupControl inputKeyGroupControlPrefab;

        [SerializeField]
        private Transform inputKeyGroupControlsLayout;

        [SerializeField]
        private Button resetButton, saveButton, backButton;

        [SerializeField]
        private GameObject interactionBlock;

        public event Action<string> OnRequestedMainKeyRebind;
        public event Action<string> OnRequestedAlternativeKeyRebind;
        public event Action<KeyCode> OnKeyListened;
        public event Action OnAppeared;
        public event UnityAction OnResetButtonClicked
        {
            add { resetButton.onClick.AddListener(value); }
            remove { resetButton.onClick.RemoveListener(value); }
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

        private List<InputKeyGroupControl> inputKeyGroupControls = new List<InputKeyGroupControl>();

        private KeyCode[] allKeyCodes;

        protected override void SJAwake()
        {
            var enumValues = Enum.GetValues(typeof(KeyCode));
            allKeyCodes = new KeyCode[enumValues.Length];

            int index = 0;
            foreach(int value in enumValues)
            {
                allKeyCodes[index] = (KeyCode)value;
                index++;
            }
        }

        protected override void SJOnEnable()
        {
            OnAppeared?.Invoke();
        }

        public void CreateInputKeyGroupControl(string name, string displayName, KeyCode main, KeyCode alternative, string mainDisplayName, string alternativeDisplayName)
        {
            var newControl = inputKeyGroupControlPrefab.Instantiate(inputKeyGroupControlsLayout);

            newControl.SetInfo(name, displayName, main, alternative, mainDisplayName, alternativeDisplayName);
            newControl.OnRequestedMainKeyRebind += CallOnRequestedMainKeyRebind;
            newControl.OnRequestedAlternativeKeyRebind += CallOnRequestedAlternativeKeyRebind;

            inputKeyGroupControls.Add(newControl);
        }

        private void CallOnRequestedMainKeyRebind(InputKeyGroupControl inputKeyGroupControl) => OnRequestedMainKeyRebind?.Invoke(inputKeyGroupControl.GroupName);
        private void CallOnRequestedAlternativeKeyRebind(InputKeyGroupControl inputKeyGroupControl) => OnRequestedAlternativeKeyRebind?.Invoke(inputKeyGroupControl.GroupName);

        protected override void SJUpdate()
        {
            for(int i = 0; i < allKeyCodes.Length; i++)
            {
                if (Input.GetKeyDown(allKeyCodes[i]))
                    OnKeyListened?.Invoke(allKeyCodes[i]);
            }
        }

        public void ListenForInputs()
        {
            EnableUpdate = true;
        }

        public void StopListeningInputs()
        {
            EnableUpdate = false;
        }

        public void UpdateMainKeyOf(string name, KeyCode main, string displayName)
        {
            ControlWithName(name).UpdateMainKey(main, displayName);
        }

        public void UpdateAlternativeKeyOf(string name, KeyCode alternative, string displayName)
        {
            ControlWithName(name).UpdateAlternativeKey(alternative, displayName);
        }

        private InputKeyGroupControl ControlWithName(string name) => inputKeyGroupControls.Find(control => control.GroupName == name);

        public void ShowConfirmationPopup(string message, Action onAccept, Action onCancel)
        {
            ConfirmationPopupProvider.ShowWith(message, onAccept, onCancel);
        }

        public void ShowAsModifiableMainOf(string name) => ControlWithName(name).ShowMainAsModifiable();
        public void ShowAsModifiableAlternativeOf(string name) => ControlWithName(name).ShowAlternativeAsModifiable();

        public void ShowAllAsNotModifiable()
        {
            for (int i = 0; i < inputKeyGroupControls.Count; i++)
            {
                inputKeyGroupControls[i].ShowMainAsNotModifiable();
                inputKeyGroupControls[i].ShowAlternativeAsNotModifiable();
            }
        }

        public void LockInteractableObjects()
        {
            interactionBlock.SetActive(true);
        }

        public void UnlockInteractableObjects()
        {
            interactionBlock.SetActive(false);
        }
    }
}