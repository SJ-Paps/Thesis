using SJ.Management;

public static class GameManagerFactory
{
    public static GameManager Create()
    {
        return new GameManager(Repositories.GetProfileRepository(), Application.Instance.ApplicationSettings());
    }
}
