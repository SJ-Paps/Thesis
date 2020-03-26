namespace SJ.Management
{
    public static class GameSettingsRepositoryFactory
    {
        public static IGameSettingsRepository Create()
        {
            return new WindowsFileSystemGameSettingsRepository();
        }
    }
}