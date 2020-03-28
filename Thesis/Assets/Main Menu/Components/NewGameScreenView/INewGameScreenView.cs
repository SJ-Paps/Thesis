using System;
using UnityEngine.Events;

namespace SJ.Menu
{
    public interface INewGameScreenView
    {
        event Action<string> OnNewProfileSubmitted;
        event UnityAction OnBackButtonClicked;

        void ShowErrorMessage(string description);
        void HideErrorMessage();
    }
}
