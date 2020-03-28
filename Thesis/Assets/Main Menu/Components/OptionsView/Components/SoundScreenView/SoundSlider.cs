using System;
using UnityEngine;
using UnityEngine.UI;

namespace SJ.Menu
{
    public class SoundSlider : SJMonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private SoundSliderHandle handle;

        public event Action<float> OnValueChanged;
        public event Action<float> OnDragEnd;

        protected override void SJAwake()
        {
            handle.OnDragEnd += CallOnDragEndEvent;
            slider.onValueChanged.AddListener(CallOnValueChangedEvent);
        }

        public void SetValue(float value) => slider.value = value;
        public float GetValue() => slider.value;

        private void CallOnValueChangedEvent(float value) => OnValueChanged?.Invoke(value);

        private void CallOnDragEndEvent() => OnDragEnd?.Invoke(slider.value);
    }
}


