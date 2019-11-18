using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SJ.Localization
{
    [CreateAssetMenu(fileName = "language_info", menuName = nameof(LanguageInfoAsset))]
    public class LanguageInfoAsset : ScriptableObject
    {
        [SerializeField]
        private LanguageInfo[] languageInfo;

        public LanguageInfo[] LanguageInfo
        {
            get
            {
                return languageInfo;
            }
        }
    }

    [Serializable]
    public class LanguageInfo
    {
        [SerializeField]
        private string language;

        [SerializeField]
        private TextAsset textAsset;

        public string Language
        {
            get
            {
                return language;
            }
        }

        public string Text
        {
            get
            {
                return textAsset.text;
            }
        }
    }
}

