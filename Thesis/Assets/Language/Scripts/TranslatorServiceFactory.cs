using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SJ.Localization
{
    public static class TranslatorServiceFactory
    {
        public static ITranslatorService Create()
        {
            var translatorService = new XmlTranslatorService(SJResources.LoadAsset<LanguageInfoAsset>(Reg.LANGUAGE_INFO_ASSET_NAME).LanguageInfo);

            return translatorService;
        }
    }
}


