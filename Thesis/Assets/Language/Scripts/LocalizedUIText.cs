using TMPro;
using UnityEngine.UI;

namespace SJ.Localization
{
    public class LocalizedUIText : LocalizedTextComponent
    {
        protected TextMeshProUGUI selfTextMeshPro;
        protected Text selfText;

        protected override void SJAwake()
        {
            EnableUpdate = false;

            selfText = GetComponent<Text>();
            selfTextMeshPro = GetComponent<TextMeshProUGUI>();

            base.SJAwake();
        }

        protected override void OnTextChanged(string text)
        {
            if (selfText != null) selfText.text = text;
            else if (selfTextMeshPro != null) selfTextMeshPro.text = text;
        }
    }
}


