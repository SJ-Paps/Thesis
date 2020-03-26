using System;
using UnityEngine;
using Application = SJ.Management.Application;

namespace SJ.Localization
{
    public enum TextOptions : byte
    {
        None,
        ToUpper,
        ToLower,
        TitleCase,
        FirstLetterToUpper
    }

    [Serializable]
    public class LocalizedText : IDisposable
    {
        public string tag;

        public string Text { get; private set; }

        private ITranslatorService translatorService;

        [SerializeField]
        private TextOptions option;

        public event Action<string> onTextChanged;

        public bool IsDisposed { get; private set; }

        public LocalizedText(string tag, TextOptions option)
        {
            this.tag = tag;
            this.option = option;

            translatorService = Application.TranslatorService;

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

                case TextOptions.TitleCase:

                    Text = translatorService.GetLineByTagOfCurrentLanguage(tag).ToTitleCase();

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


