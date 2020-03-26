using System;
using UniRx;

namespace SJ.Management
{
    public interface IGameSettingsRepository
    {
        IObservable<GameSettings> GetSettings();
        IObservable<Unit> SaveSettings();
    }
}