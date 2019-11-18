using System.Collections.Generic;
using System.Xml;
using System;
using System.Linq;

namespace SJ.Localization
{
    public class XmlTranslatorService : ITranslatorService
    {
        private class LocalizedTextContainer
        {
            private string language;

            private Dictionary<string, string> lines;

            public LocalizedTextContainer(string language, string xml)
            {
                lines = new Dictionary<string, string>();

                this.language = language;

                ExtractLinesFrom(xml);
            }

            private void ExtractLinesFrom(string xml)
            {
                XmlDocument langXml = new XmlDocument();

                langXml.LoadXml(xml);

                XmlNodeList nodes = langXml.SelectNodes("language/lines/line");

                foreach (XmlNode lineInfo in nodes)
                {
                    lines.Add(lineInfo.Attributes["tag"].Value, lineInfo.InnerText);
                }
            }

            public string GetLineByTagAttribute(string tag)
            {
                if (lines.ContainsKey(tag))
                {
                    return lines[tag];
                }

                Logger.LogConsole("TAG NOT FOUND! REMEMBER BUILD ASSETBUNDLES!");

                return string.Empty;
            }
        }

        public string CurrentLanguage { get; private set; }

        private Dictionary<string, LocalizedTextContainer> localizedTextContainers;

        public event Action<string> onLanguageChanged;

        public XmlTranslatorService(LanguageInfo[] languageInfo)
        {
            localizedTextContainers = new Dictionary<string, LocalizedTextContainer>();

            for (int i = 0; i < languageInfo.Length; i++)
            {
                localizedTextContainers.Add(languageInfo[i].Language, new LocalizedTextContainer(languageInfo[i].Language, languageInfo[i].Text));
            }

            CurrentLanguage = languageInfo[0].Language;
        }

        private bool ContainsLanguage(string language)
        {
            string[] allLanguages = GetLanguages();

            foreach(string savedLanguage in allLanguages)
            {
                if(savedLanguage == language)
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangeLanguage(string language)
        {
            if(ContainsLanguage(language))
            {
                CurrentLanguage = language;
            }

            throw new InvalidOperationException("language " + language + " is invalid");
        }

        public string[] GetLanguages()
        {
            return localizedTextContainers.Keys.ToArray();
        }

        public string GetLineByTagOfCurrentLanguage(string tag)
        {
            return localizedTextContainers[CurrentLanguage].GetLineByTagAttribute(tag);
        }

        public string GetLineByTagOfLanguage(string language, string tag)
        {
            return localizedTextContainers[language].GetLineByTagAttribute(tag);
        }
    }
}


