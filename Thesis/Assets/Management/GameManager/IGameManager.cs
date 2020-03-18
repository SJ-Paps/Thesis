using SJ.GameEntities;
using System;

namespace SJ.Management
{
    public interface IGameManager
    {
        event Action OnSaving;
        event Action OnSaveFailed;
        event Action OnSaveSucceeded;

        event Action OnLoading;
        event Action OnLoadingFailed;
        event Action OnLoadingSucceeded;

        event Action OnSessionBegan;
        event Action OnSessionFinished;

        string CurrentProfile { get; }

        bool IsInGame();

        void BeginSessionFor(string profile);
        void EndSession();
        void Reload();
        void Save();

        void SubscribeSaveable(ISaveableGameEntity saveable);
        void UnsubscribeSaveable(ISaveableGameEntity saveable);
    }
}