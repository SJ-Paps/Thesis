using System;
using UnityEngine.Events;

namespace SJ.Menu
{
    public interface ILoadGameScreenView
    {
        event Action OnAppeared;
        event Action<string> OnProfileSelectClicked;
        event Action<string> OnProfileDeleteClicked;
        event UnityAction OnBackButtonClicked;

        void ShowProfiles(string[] profiles);
    }
}