using System.Xml;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedTextLibrary
{
    #region LOCALIZEDTEXTCONTAINER
    private class LocalizedTextContainer
    {
        private Language language;

        private XmlNodeList lines;

        public LocalizedTextContainer(Language language)
        {
            this.language = language;

            AssetBundle languageBundle = AssetBundleLibrary.Instance.GetAssetBundleByNameWithExtension(Reg.languageFileName + language.extension);

            TextAsset languageLines = languageBundle.LoadAsset<TextAsset>(Reg.languageAssetName);

            ExtractLinesFrom(languageLines);
        }

        private void ExtractLinesFrom(TextAsset languageLines)
        {
            string xmlText = languageLines.text;

            XmlDocument langXml = new XmlDocument();

            langXml.LoadXml(xmlText);

            lines = langXml.SelectNodes("language/lines/line");
        }

        public string GetLineByTagAttribute(string tag)
        {
            foreach (XmlNode lineInfo in lines)
            {
                if (lineInfo.Attributes["tag"].Value == tag)
                {
                    return lineInfo.InnerText;
                }
            }

            Logger.LogConsole("TAG NOT FOUND! REMEMBER BUILD ASSETBUNDLES!");

            return string.Empty;
        }
    }
    #endregion

    #region SINGLETON_PATTERN
    private static LocalizedTextLibrary instance;

    public static LocalizedTextLibrary GetInstance()
    {
        if (instance == null)
        {
            instance = new LocalizedTextLibrary();
        }

        return instance;
    }
    #endregion

    private Dictionary<Language, LocalizedTextContainer> localizedTextContainers;

    private LocalizedTextContainer current;

    private LanguageManager languageManager;

    private LocalizedTextLibrary()
    {
        languageManager = LanguageManager.GetInstance();

        Language[] languages = languageManager.GetLanguages();

        localizedTextContainers = new Dictionary<Language, LocalizedTextContainer>();

        for (int i = 0; i < languages.Length; i++)
        {
            localizedTextContainers.Add(languages[i], new LocalizedTextContainer(languages[i]));
        }

        current = localizedTextContainers[LanguageManager.GetInstance().CurrentLanguage];

        languageManager.onLanguageChanged += OnLanguageChanged;
    }

    public string GetLineByTagAttribute(string tag)
    {
        return current.GetLineByTagAttribute(tag);
    }

    private void OnLanguageChanged(Language language)
    {
        current = localizedTextContainers[language];
    }
}
