using System;
using UnityEngine.Events;

namespace SJ.Menu
{
    public interface ISoundSettingsScreenView
    {
        event Action<float> OnGeneralSoundValueChanged;
        event Action<float> OnGeneralSoundValueConfirmed;

        event Action<float> OnMusicSoundValueChanged;
        event Action<float> OnMusicSoundValueConfirmed;

        event Action<float> OnEffectsSoundValueChanged;
        event Action<float> OnEffectsSoundValueConfirmed;

        event UnityAction OnBackButtonClicked;

        void SetGeneralSoundValue(float value);
        void SetMusicSoundValue(float value);
        void SetEffectsSoundValue(float value);
    }

}