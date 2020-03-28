using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class LanguageSettingsScreenView : SJMonoBehaviour, ILanguageSettingsScreenView
    {
        [SerializeField]
        private Button languageButtonPrefab;

        [SerializeField]
        private Button backButton;

        [SerializeField]
        private Transform buttonLayout;

        private Dictionary<string, TextMeshProUGUI> buttonTexts = new Dictionary<string, TextMeshProUGUI>();

        public event Action<string> OnLanguageSelected;

        public event UnityAction OnBackButtonClicked
        {
            add { backButton.onClick.AddListener(value); }
            remove { backButton.onClick.RemoveListener(value); }
        }

        public void AddLanguageButtons(Dictionary<string, string> translatedButtonTexts)
        {
            foreach (var translatedText in translatedButtonTexts)
                CreateLanguageButtonAndSaveTextComponent(translatedText.Key, translatedText.Value);
        }

        private void CreateLanguageButtonAndSaveTextComponent(string language, string text)
        {
            var button = Instantiate(languageButtonPrefab, buttonLayout);
            var textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
            
            textComponent.text = text;
            button.onClick.AddListener(() => OnLanguageSelected?.Invoke(language));

            buttonTexts.Add(language, textComponent);
        }

        public void UpdateLanguageButtonTexts(Dictionary<string, string> translatedButtonTexts)
        {
            foreach (var translatedText in translatedButtonTexts)
                buttonTexts[translatedText.Key].text = translatedText.Value;
        }
    }
}

