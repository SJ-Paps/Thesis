using System.Xml;
using UnityEngine;
using System;

public struct Language
{
    public readonly string name;
    public readonly string extension;

    public Language(string name, string extension)
    {
        this.name = name;
        this.extension = extension;
    }

    public override string ToString()
    {
        return name;
    }
}

public class LanguageManager
{
    private static LanguageManager instance;

    public static LanguageManager GetInstance()
    {
        if(instance == null)
        {
            instance = new LanguageManager();
        }

        return instance;
    }

    private Language[] languages;
    private Language current;

    public Language CurrentLanguage
    {
        get
        {
            return current;
        }
    }

    public event Action<Language> onLanguageChanged;

    public LanguageManager()
    {
        AssetBundle langListBundle = AssetBundleLibrary.Instance.GetAssetBundleByNameWithoutExtension(Reg.languageListFileName);

        TextAsset langList = langListBundle.LoadAsset<TextAsset>(Reg.languageListAssetName);

        languages = GetLanguagesFrom(langList);

        current = GetPlayerLanguage();
    }

    private Language[] GetLanguagesFrom(TextAsset languageList)
    {
        string xmlText = languageList.text;

        XmlDocument langXml = new XmlDocument();

        langXml.LoadXml(xmlText);

        XmlNodeList languages = langXml.SelectNodes("/languages/language");

        Language[] obtainedLanguages = new Language[languages.Count];

        for (int i = 0; i < obtainedLanguages.Length; i++)
        {
            obtainedLanguages[i] = new Language(languages[i].Attributes["name"].Value,
                                                "." + languages[i].Attributes["extension"].Value);
        }

        return obtainedLanguages;

    }

    private Language GetPlayerLanguage()
    {
        return GetDefaultLanguage();
    }

    private Language GetDefaultLanguage()
    {
        return languages[0];
    }

    public Language[] GetLanguages()
    {
        return languages;
    }

    public void ChangeLanguage(string language)
    {
        foreach (Language lang in languages)
        {
            if (lang.name == language)
            {
                current = lang;

                if (onLanguageChanged != null)
                {
                    onLanguageChanged(lang);
                }

                break;
            }
        }
    }
}