using UnityEngine;

namespace SJ.Management.Localization
{
    public abstract class LocalizedTextComponent : SJMonoBehaviour
    {
        [SerializeField]
        protected string langTag;

        [SerializeField]
        protected TextOptions option;

        protected LocalizedText localizedText;

        public string Text
        {
            get
            {
                return localizedText.ToString();
            }
        }

        public string LanguageTag
        {
            get => localizedText.tag;
            set
            {
                localizedText.tag = langTag;

                localizedText.UpdateText();
            }
        }

        protected override void SJAwake()
        {
            localizedText = new LocalizedText(langTag, option);

            localizedText.onTextChanged += OnTextChanged;

            localizedText.UpdateText();
        }

        protected override void SJOnDestroy()
        {
            localizedText.Dispose();
        }

        protected abstract void OnTextChanged(string text);
    }
}


