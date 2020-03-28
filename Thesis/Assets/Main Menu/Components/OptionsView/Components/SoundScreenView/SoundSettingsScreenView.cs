using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class SoundSettingsScreenView : SJMonoBehaviour, ISoundSettingsScreenView
    {
        [SerializeField]
        private SoundSlider generalSoundSlider, musicSoundSlider, effectsSoundSlider;

        [SerializeField]
        private Button backButton;

        public event Action<float> OnGeneralSoundValueChanged
        {
            add { generalSoundSlider.OnValueChanged += value; }
            remove { generalSoundSlider.OnValueChanged -= value; }
        }
        public event Action<float> OnGeneralSoundValueConfirmed
        {
            add { generalSoundSlider.OnDragEnd += value; }
            remove { generalSoundSlider.OnDragEnd -= value; }
        }
        public event Action<float> OnMusicSoundValueChanged
        {
            add { musicSoundSlider.OnValueChanged += value; }
            remove { musicSoundSlider.OnValueChanged -= value; }
        }
        public event Action<float> OnMusicSoundValueConfirmed
        {
            add { musicSoundSlider.OnDragEnd += value; }
            remove { musicSoundSlider.OnDragEnd -= value; }
        }
        public event Action<float> OnEffectsSoundValueChanged
        {
            add { effectsSoundSlider.OnValueChanged += value; }
            remove { effectsSoundSlider.OnValueChanged -= value; }
        }
        public event Action<float> OnEffectsSoundValueConfirmed
        {
            add { effectsSoundSlider.OnDragEnd += value; }
            remove { effectsSoundSlider.OnDragEnd -= value; }
        }
        public event UnityAction OnBackButtonClicked
        {
            add { backButton.onClick.AddListener(value); }
            remove { backButton.onClick.RemoveListener(value); }
        }

        public void SetEffectsSoundValue(float value) => effectsSoundSlider.SetValue(value);

        public void SetGeneralSoundValue(float value) => generalSoundSlider.SetValue(value);

        public void SetMusicSoundValue(float value) => musicSoundSlider.SetValue(value);
    }
}