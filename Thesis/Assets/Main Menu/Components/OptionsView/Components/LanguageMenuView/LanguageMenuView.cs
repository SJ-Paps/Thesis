using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class LanguageMenuView : SJMonoBehaviour, ILanguageMenuView
    {
        [SerializeField]
        private Button languageButtonPrefab;

        [SerializeField]
        private Transform buttonLayout;

        private Dictionary<string, TextMeshProUGUI> buttonTexts = new Dictionary<string, TextMeshProUGUI>();

        public event Action<string> OnLanguageSelected;

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

