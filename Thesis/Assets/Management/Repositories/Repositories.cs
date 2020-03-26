namespace SJ.Management
{
    public static class Repositories
    {
        private static IProfileRepository profileRepository;
        private static IGameSettingsRepository gameSettingsRepository;
        private static IGameInputSettingsRepository gameInputSettingsRepository;

        public static IProfileRepository GetProfileRepository()
        {
            if(profileRepository == null)
                profileRepository = ProfileRepositoryFactory.Create();

            return profileRepository;
        }

        public static IGameSettingsRepository GetGameSettingsRepository()
        {
            if(gameSettingsRepository == null)
                gameSettingsRepository = GameSettingsRepositoryFactory.Create();

            return gameSettingsRepository;
        }

        public static IGameInputSettingsRepository GetGameInputSettingsRepository()
        {
            if (gameInputSettingsRepository == null)
                gameInputSettingsRepository = GameInputSettingsRepositoryFactory.Create();

            return gameInputSettingsRepository;
        }
    }
}