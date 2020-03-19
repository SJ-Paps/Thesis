using System;

namespace SJ.Menu
{
    public interface ISoundMenuView
    {
        event Action<float> OnGeneralSoundValueChanged;
        event Action<float> OnGeneralSoundValueConfirmed;

        event Action<float> OnMusicSoundValueChanged;
        event Action<float> OnMusicSoundValueConfirmed;

        event Action<float> OnEffectsSoundValueChanged;
        event Action<float> OnEffectsSoundValueConfirmed;

        void SetGeneralSoundValue(float value);
        void SetMusicSoundValue(float value);
        void SetEffectsSoundValue(float value);
    }

}