using SJ.Management;

namespace SJ.Management.Localization
{
    public static class TranslatorServiceFactory
    {
        public static ITranslatorService Create()
        {
            return new XmlTranslatorService(SJResources.LoadAsset<LanguageSettings>(Reg.LanguageSettingsAssetName).LanguageInfo);
        }
    }
}


