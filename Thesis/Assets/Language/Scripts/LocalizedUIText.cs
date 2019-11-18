﻿using UnityEngine.UI;

namespace SJ.Localization
{
    public class LocalizedUIText : LocalizedTextComponent
    {

        protected Text selfText;

        protected override void SJAwake()
        {

            selfText = GetComponent<Text>();

            base.SJAwake();
        }

        protected override void OnTextChanged(string text)
        {
            selfText.text = text;
        }
    }
}


