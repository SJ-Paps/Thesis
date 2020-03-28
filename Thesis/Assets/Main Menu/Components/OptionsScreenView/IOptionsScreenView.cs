using UnityEngine.Events;

namespace SJ.Menu
{
    public interface IOptionsScreenView
    {
        event UnityAction OnGoToSoundsSettingsButtonClicked;
        event UnityAction OnGoToLanguageSettingsButtonClicked;
        event UnityAction OnGoToGameInputSettingsButtonClicked;
        event UnityAction OnBackButtonClicked;

        void FocusSoundSettingsScreen();
        void FocusLanguageSettingsScreen();
        void FocusGameInputSettingsScreen();

        void FocusMainScreen();
    }
}