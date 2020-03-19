using System;

namespace SJ.Menu
{
    public interface INewGameScreenView
    {
        event Action<string> OnNewProfileSubmitted;

        void ShowErrorMessage(string description);
        void HideErrorMessage();
    }
}
