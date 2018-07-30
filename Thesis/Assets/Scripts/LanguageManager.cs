using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct LanguageInfo
{
    public readonly string name;
    public readonly string extension;
    public readonly string shortName;

    public LanguageInfo(string _name, string _extension, string _shortName)
    {
        name = _name;
        extension = _extension;
        shortName = _shortName;
    }

    public override string ToString()
    {
        return name;
    }
}

public class LanguageManager
{
    #region SINGLETON_PATTERN
    private static LanguageManager instance;

    public static LanguageManager GetInstance()
    {
        if (instance == null)
        {
            instance = new LanguageManager();
        }

        return instance;
    }
    #endregion

    #region LANGUAGE
    private class Language
    {
        public static readonly string nameAttributeIdentifier = "name";
        public static readonly string extensionAttributeIdentifier = "extension";
        public static readonly string shortNameAttributeIdentifier = "short";

        public readonly string name;
        public readonly string extension;
        public readonly string shortName;

        private XmlNodeList lines;

        public Language(string _name, string _extension, string _shortName)
        {
            name = _name;
            extension = _extension;
            shortName = _shortName;
        }

        public override string ToString()
        {
            return name;
        }

        public LanguageInfo GetInfo()
        {
            return new LanguageInfo(name, extension, shortName);
        }

        public void ExtractLinesFrom(TextAsset languageLines)
        {
            string xmlText = languageLines.text;

            XmlDocument langXml = new XmlDocument();

            langXml.LoadXml(xmlText);

            lines = langXml.SelectNodes("/lines/line");
        }

        public string GetLineByTagAttribute(string tag)
        {
            foreach(XmlNode lineInfo in lines)
            {
                if(lineInfo.Attributes["tag"].Value == tag)
                {
                    return lineInfo.InnerText;
                }
            }

            return string.Empty;
        }
    }
    #endregion

    public event Action<LanguageInfo> onLanguageChanged;

    private Language current;

    public LanguageInfo Current
    {
        get
        {
            if(current != null)
            {
                return current.GetInfo();
            }

            return default(LanguageInfo);
        }
    }

    private List<Language> languages;

    private LanguageManager()
    {
        languages = new List<Language>();

        AssetBundle langListBundle = AssetBundleLibrary.Instance.GetAssetBundleByNameWithoutExtension(Reg.languageListFileName);

        if(langListBundle != null)
        {
            TextAsset langList = langListBundle.LoadAsset<TextAsset>(Reg.languageListAssetName);

            try
            {
                languages.AddRange(GetLanguagesFrom(langList));

                for (int i = 0; i < languages.Count; i++)
                {
                    AssetBundle languageBundle = AssetBundleLibrary.Instance.GetAssetBundleByFileExtension(languages[i].extension);

                    if (languageBundle != null)
                    {
                        TextAsset languangeTextLines = languageBundle.LoadAsset<TextAsset>(Reg.languageAssetName);

                        languages[i].ExtractLinesFrom(languangeTextLines);
                    }
                    else
                    {
                        languages.RemoveAt(i);
                        i--;
                    }
                }
            }
            catch(Exception e)
            {
                EditorDebug.Log(e);
                //Que hacemos si los archivos de idioma tienen algun problema?
                
            }
            
        }

        current = GetPlayerLanguage();
    }

    private Language[] GetLanguagesFrom(TextAsset xmlAsset)
    {
        string xmlText = xmlAsset.text;

        XmlDocument langXml = new XmlDocument();

        langXml.LoadXml(xmlText);

        XmlNodeList languages = langXml.SelectNodes("/languages/language");

        Language[] obtainedLanguages = new Language[languages.Count];

        for(int i = 0; i < obtainedLanguages.Length; i++)
        {
            obtainedLanguages[i] = new Language(languages[i].Attributes[Language.nameAttributeIdentifier].Value,
                                                languages[i].Attributes[Language.extensionAttributeIdentifier].Value,
                                                languages[i].Attributes[Language.shortNameAttributeIdentifier].Value);
        }

        return obtainedLanguages;
        
    }

    private Language GetPlayerLanguage()
    {
        return GetDefaultLanguage();
    }

    private Language GetDefaultLanguage()
    {
        if(languages != null && languages.Count != 0)
        {
            return languages[0];
        }

        return null;
    }

    public string GetLineByTagAttribute(string tag)
    {
        return current.GetLineByTagAttribute(tag);
    }

    public LanguageInfo[] GetLanguages()
    {
        LanguageInfo[] info = new LanguageInfo[languages.Count];

        for(int i = 0; i < info.Length; i++)
        {
            info[i] = languages[i].GetInfo();
        }

        return info;
    }

    public void ChangeLanguage(LanguageInfo info)
    {
        foreach(Language lang in languages)
        {
            if(lang.name == info.name)
            {
                current = lang;

                if(onLanguageChanged != null)
                {
                    onLanguageChanged(info);
                }

                break;
            }
        }
    }
}
