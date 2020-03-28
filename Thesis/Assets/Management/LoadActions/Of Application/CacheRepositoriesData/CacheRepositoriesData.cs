using System;
using UniRx;

namespace SJ.Management
{
    public class CacheRepositoriesData : LoadAction
    {
        protected override IObservable<Unit> Load()
        {
            return Repositories.GetGameSettingsRepository()
                .GetSettings()
                .ContinueWith(_ => Repositories.GetGameInputSettingsRepository().GetSettings())
                .ContinueWith(_ => Repositories.GetProfileRepository().GetAllProfiles())
                .ObserveOnMainThread()
                .AsUnitObservable();
        }
    }
}