namespace SJ.Management
{
    public static class UpdaterFactory
    {
        public static IUpdater Create()
        {
            return new Updater();
        }
    }
}


