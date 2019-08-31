public static class ApplicationInfo
{
    private static ApplicationInfoAsset infoAsset;

    static ApplicationInfo()
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

    public static GameConfiguration DefaultGameConfiguration
    {
        get
        {
            return infoAsset.DefaultGameConfiguration;
        }
    }
}
