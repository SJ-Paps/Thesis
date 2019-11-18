using System;
using UnityEngine;

namespace SJ.Localization
{
    public enum TextOptions : byte
    {
        None,
        ToUpper,
        ToLower,
        FirstLetterToUpper
    }

    [Serializable]
    public class LocalizedText : IDisposable
    {
        public string tag;

        private string text;

        public string Text
        {
            get
            {
                return text;
            }

            private set
            {
                text = value;
            }
        }

        private ITranslatorService translatorService;

        [SerializeField]
        private TextOptions option;

        public event Action<string> onTextChanged;

        public bool IsDisposed { get; private set; }

        public LocalizedText(string tag, TextOptions option)
        {
            this.tag = tag;
            this.option = option;

            translatorService = Application.GetTranslatorService();

            translatorService.onLanguageChanged += OnLanguageChanged;
        }

        public void Dispose()
        {
            if (IsDisposed == false)
            {
                translatorService.onLanguageChanged -= OnLanguageChanged;
                IsDisposed = true;
            }
        }

        private void OnLanguageChanged(string language)
        {
            UpdateText();
        }

        public void UpdateText()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(LocalizedText) + " TAG: " + tag);
            }

            switch (option)
            {
                case TextOptions.None:

                    Text = translatorService.GetLineByTagOfCurrentLanguage(tag);

                    break;

                case TextOptions.ToLower:

                    Text = translatorService.GetLineByTagOfCurrentLanguage(tag).ToLower();

                    break;

                case TextOptions.ToUpper:

                    Text = translatorService.GetLineByTagOfCurrentLanguage(tag).ToUpper();

                    break;

                case TextOptions.FirstLetterToUpper:

                    Text = translatorService.GetLineByTagOfCurrentLanguage(tag).FirstLetterToUpper();

                    break;
            }

            onTextChanged(Text);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}


