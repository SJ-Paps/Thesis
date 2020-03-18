using System;
using UnityEngine.Events;

namespace SJ.UI
{
    public interface IMainScreenView
    {
        event UnityAction OnNewGameClicked;
        event UnityAction OnLoadGameClicked;
        event UnityAction OnContinueClicked;
        event UnityAction OnResumeGameClicked;
        event UnityAction OnOptionsClicked;
        event UnityAction OnExitToDesktopClicked;
        event UnityAction OnExitToMainMenuClicked;

        event Action OnAppeared;

        void ShowInGameButtons();
        void ShowInMenuButtons();
        void ShowContinueButton();
        void HideContinueButton();
    }
}