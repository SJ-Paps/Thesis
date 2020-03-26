namespace SJ.Management
{
    public static class GameInputSettingsRepositoryFactory
    {
        public static IGameInputSettingsRepository Create()
        {
            return new WindowsFileSystemGameInputSettingsRepository();
        }
    }
}


