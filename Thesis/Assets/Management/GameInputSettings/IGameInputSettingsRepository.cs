using System;
using UniRx;

namespace SJ.Management
{
    public interface IGameInputSettingsRepository
    {
        IObservable<GameInputSettings> GetSettings();
        IObservable<Unit> SaveSettings();
    }
}