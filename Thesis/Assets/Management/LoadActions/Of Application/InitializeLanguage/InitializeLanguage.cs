using System;
using UniRx;

namespace SJ.Management
{
    public class InitializeLanguage : LoadAction
    {
        protected override IObservable<Unit> Load()
        {
            return Repositories.GetGameSettingsRepository()
                .GetSettings()
                .Do(settings => Application.TranslatorService.ChangeLanguage(settings.userLanguage))
                .AsUnitObservable();
        }
    }
}