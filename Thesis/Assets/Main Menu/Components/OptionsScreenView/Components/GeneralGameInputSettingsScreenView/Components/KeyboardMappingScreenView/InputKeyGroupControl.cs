using SJ.Tools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class InputKeyGroupControl : SJMonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nameText, mainKeyText, alternativeKeyText;

        [SerializeField]
        private DoubleClickGraphic mainKeyDoubleClickGraphic, alternativeKeyDoubleClickGraphic;

        [SerializeField]
        private Image mainModifiableImageFeedback, alternativeModifiableImageFeedback;

        public string GroupName { get; private set; }

        public KeyCode Main { get; private set; }
        public KeyCode Alternative { get; private set; }

        public Action<InputKeyGroupControl> OnRequestedMainKeyRebind;
        public Action<InputKeyGroupControl> OnRequestedAlternativeKeyRebind;

        protected override void SJAwake()
        {
            mainKeyDoubleClickGraphic.OnTriggered += OnMainKeyDoubleClick;
            alternativeKeyDoubleClickGraphic.OnTriggered += OnAlternativeKeyDoubleClick;
        }

        public void SetInfo(string name, string displayName, KeyCode main, KeyCode alternative, string mainDisplayName, string alternativeDisplayName)
        {
            GroupName = name;
            nameText.text = displayName;
            Main = main;
            Alternative = alternative;
            mainKeyText.text = mainDisplayName;
            alternativeKeyText.text = alternativeDisplayName;
        }

        public void UpdateMainKey(KeyCode main, string displayName)
        {
            Main = main;
            mainKeyText.text = displayName;
        }

        public void UpdateAlternativeKey(KeyCode alternative, string displayName)
        {
            Alternative = alternative;
            alternativeKeyText.text = displayName;
        }

        public void ShowMainAsModifiable() => mainModifiableImageFeedback.color = Color.red;
        public void ShowMainAsNotModifiable() => mainModifiableImageFeedback.color = Color.black;
        public void ShowAlternativeAsModifiable() => alternativeModifiableImageFeedback.color = Color.red;
        public void ShowAlternativeAsNotModifiable() => alternativeModifiableImageFeedback.color = Color.black;

        private void OnMainKeyDoubleClick() => OnRequestedMainKeyRebind?.Invoke(this);

        private void OnAlternativeKeyDoubleClick() => OnRequestedAlternativeKeyRebind?.Invoke(this);
    }
}