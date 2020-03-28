using System;
using UnityEngine;

namespace SJ.Management.Localization
{
    [CreateAssetMenu(fileName = Reg.LanguageSettingsAssetName, menuName = "Language Settings Asset")]
    public class LanguageSettings : ScriptableObject
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

