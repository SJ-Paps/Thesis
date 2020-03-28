using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SJ.Menu
{
    public interface ILanguageSettingsScreenView
    {
        event UnityAction OnBackButtonClicked;
        event Action<string> OnLanguageSelected;

        void AddLanguageButtons(Dictionary<string, string> translatedButtonText);
        void UpdateLanguageButtonTexts(Dictionary<string, string> translatedButtonTexts);
    }
}

