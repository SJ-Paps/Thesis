using System;

namespace SJ.Menu
{
    public interface ILoadGameScreenView
    {
        event Action OnAppeared;
        event Action<string> OnProfileSelectClicked;
        event Action<string> OnProfileDeleteClicked;

        void ShowProfiles(string[] profiles);
    }
}