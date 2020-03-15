namespace SJ.Localization
{
    public static class TranslatorServiceFactory
    {
        public static ITranslatorService Create()
        {
            var translatorService = new XmlTranslatorService(SJResources.LoadAsset<LanguageSettings>(Reg.LanguageSettingsAssetName).LanguageInfo);

            return translatorService;
        }
    }
}


