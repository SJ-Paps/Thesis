public class InitializeSound : ScriptableLoadRoutine
{
    public override bool IsCompleted
    {
        get
        {
            return true;
        }
    }

    public override bool IsFaulted
    {
        get
        {
            return false;
        }
    }

    public override void Load()
    {
        GameConfiguration gameConfiguration = GameConfigurationCareTaker.GetConfiguration();

        SoundManager.GetInstance().ChangeVolume(gameConfiguration.generalVolume);
        SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Music].ChangeVolume(gameConfiguration.musicVolume);
        SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Effects].ChangeVolume(gameConfiguration.soundsVolume);
    }

    public override bool ShouldRetry()
    {
        return false;
    }
}
