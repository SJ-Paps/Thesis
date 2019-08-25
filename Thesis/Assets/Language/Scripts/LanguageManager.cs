using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class LanguageManager
{
    private static Dictionary<string, LanguageInfo> languageInfo;

    public static string CurrentLanguage { get; private set; }

    public static event Action<string> onLanguageChanged;

    private static ILocalizedTextLibrary localizedTextLibrary;

    static LanguageManager()
    {
        languageInfo = new Dictionary<string, LanguageInfo>();
    }

    public static void LoadLanguageInfo(LanguageInfo[] info, int defaultIndex)
    {
        for(int i = 0; i < info.Length; i++)
        {
            languageInfo.Add(info[i].Language, info[i]);
        }

        ChangeLanguage(info[defaultIndex].Language);
    }

    public static string[] GetLanguages()
    {
        string[] languages = new string[languageInfo.Count];

        int i = 0;

        foreach(string key in languageInfo.Keys)
        {
            languages[i] = key;
            i++;
        }

        return languages;
    }

    public static void ChangeLanguage(string language)
    {
        if(languageInfo.ContainsKey(language))
        {
            CurrentLanguage = language;

            NotifyLanguageChanged();
            
        }
    }

    private static void NotifyLanguageChanged()
    {
        if(onLanguageChanged != null)
        {
            onLanguageChanged(CurrentLanguage);
        }
    }

    public static ILocalizedTextLibrary GetLocalizedTextLibrary()
    {
        if(localizedTextLibrary == null)
        {
            localizedTextLibrary = LocalizedTextLibraryFactory();
        }

        return localizedTextLibrary;
    }

    private static ILocalizedTextLibrary LocalizedTextLibraryFactory()
    {
        return new LocalizedTextLibrary(languageInfo.Values.ToArray());
    }
}