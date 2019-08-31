using System;

public class InitializeLanguage : ScriptableLoadRoutine
{

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
        LanguageInfo[] languageInfo = SJResources.LoadAsset<LanguageInfoAsset>(Reg.LANGUAGE_INFO_ASSET_NAME).LanguageInfo;

        LanguageManager.LoadLanguageInfo(languageInfo, GetDefaultLanguageIndex(languageInfo));
    }

    private int GetDefaultLanguageIndex(LanguageInfo[] languageInfo)
    {
        string defaultLanguage = GameConfigurationCareTaker.GetConfiguration().userLanguage;

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
