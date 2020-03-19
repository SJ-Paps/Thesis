using System;
using System.Collections.Generic;

namespace SJ.Menu
{
    public interface ILanguageMenuView
    {
        event Action<string> OnLanguageSelected;

        void AddLanguageButtons(Dictionary<string, string> translatedButtonText);
        void UpdateLanguageButtonTexts(Dictionary<string, string> translatedButtonTexts);
    }
}

