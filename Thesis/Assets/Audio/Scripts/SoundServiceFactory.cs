using SJ.Management;
using UniRx;

namespace SJ.Audio
{
    public static class SoundServiceFactory
    {
        public static ISoundService Create()
        {
            var gameSettings = Repositories.GetGameSettingsRepository()
                .GetSettings()
                .Wait();

            return new SoundService(gameSettings.generalVolume, gameSettings.musicVolume, gameSettings.effectsVolume);
        }
    }

}

