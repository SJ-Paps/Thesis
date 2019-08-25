using System;

public class InitializeLanguage : ScriptableLoadRoutine
{
    private const string LANGUAGE_INFO_ASSET_NAME = "language_info";

    public override bool IsCompleted
    {
        get
        {
            return true;
        }
    }

    public override bool IsFaulted
    {
        get
        {
            return false;
        }
    }

    public override void Load()
    {
        LanguageInfo[] languageInfo = SJResources.LoadAsset<LanguageInfoAsset>(LANGUAGE_INFO_ASSET_NAME).LanguageInfo;

        LanguageManager.LoadLanguageInfo(languageInfo, GetDefaultLanguageIndex(languageInfo));
    }

    private int GetDefaultLanguageIndex(LanguageInfo[] languageInfo)
    {
        string defaultLanguage = ApplicationInfo.DefaultGameConfiguration.defaultLanguage;

        for(int i = 0; i < languageInfo.Length; i++)
        {
            if(languageInfo[i].Language == defaultLanguage)
            {
                return i;
            }
        }

        throw new InvalidOperationException("default langauge mismatch");
    }

    public override bool ShouldRetry()
    {
        return false;
    }
}
