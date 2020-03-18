using System;

namespace SJ.UI
{
    public interface IMainMenu
    {
        void FocusMainScreen();
        void FocusOptionsScreen();
        void FocusNewGameScreen();
        void FocusLoadGameScreen();
        void Hide();
        void Show();
        void ShowConfirmationPopup(string message, Action onAccept, Action onCancel);
    }
}