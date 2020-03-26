using SJ.Management;

namespace SJ.Localization
{
    public static class TranslatorServiceFactory
    {
        public static ITranslatorService Create()
        {
            return new XmlTranslatorService(SJResources.LoadAsset<LanguageSettings>(Reg.LanguageSettingsAssetName).LanguageInfo);
        }
    }
}


