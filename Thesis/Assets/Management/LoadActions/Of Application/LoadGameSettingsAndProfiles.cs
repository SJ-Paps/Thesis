using System;
using UniRx;

namespace SJ.Management
{
    public class LoadGameSettingsAndProfiles : LoadAction
    {
        protected override IObservable<Unit> Load()
        {
            return Repositories.GetGameSettingsRepository()
                .GetSettings()
                .ContinueWith(_ => Repositories.GetProfileRepository().GetAllProfiles())
                .ObserveOnMainThread()
                .AsUnitObservable();
        }
    }
}