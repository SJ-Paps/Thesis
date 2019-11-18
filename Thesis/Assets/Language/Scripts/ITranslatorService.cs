using System;

namespace SJ.Localization
{
    public interface ITranslatorService
    {
        string CurrentLanguage { get; }

        event Action<string> onLanguageChanged;

        string[] GetLanguages();
        void ChangeLanguage(string language);
        string GetLineByTagOfCurrentLanguage(string tag);
        string GetLineByTagOfLanguage(string language, string tag);
    }

}