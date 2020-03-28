using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class NewGameScreenView : SJMonoBehaviour, INewGameScreenView
    {
        [SerializeField]
        private TMP_InputField inputField;

        [SerializeField]
        private Button submitButton, backButton;

        [SerializeField]
        private TextMeshProUGUI notificationText;

        public event Action<string> OnNewProfileSubmitted;

        public event UnityAction OnBackButtonClicked
        {
            add { backButton.onClick.AddListener(value); }
            remove { backButton.onClick.RemoveListener(value); }
        }

        protected override void SJAwake()
        {
            submitButton.onClick.AddListener(() => OnNewProfileSubmitted?.Invoke(inputField.text));
        }

        public void ShowErrorMessage(string message)
        {
            notificationText.gameObject.SetActive(true);
            notificationText.text = message;
        }

        public void HideErrorMessage()
        {
            notificationText.gameObject.SetActive(false);
        }
    }
}
