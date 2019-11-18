namespace SJ
{
    public static class ApplicationInfo
    {
        private static ApplicationInfoAsset infoAsset;

        public static void Load()
        {
            infoAsset = SJResources.LoadAsset<ApplicationInfoAsset>(Reg.APPLICATION_INFO_ASSET_NAME);
        }

        public static string[] BeginningScenes
        {
            get
            {
                return infoAsset.BeginningScenes;
            }
        }

        public static string ReturnSceneOnEndSession
        {
            get
            {
                return infoAsset.ReturnSceneOnEndSession;
            }
        }

        public static GameSettings DefaultGameConfiguration
        {
            get
            {
                return infoAsset.DefaultGameConfiguration;
            }
        }
    }

}