using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace SJ.Menu
{
    public class SoundSlider : SJMonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private SoundSliderHandle handle;

        public event Action<float> onValueChanged;
        public event Action onDragEnd
        {
            add
            {
                handle.onDragEnd += value;
            }

            remove
            {
                handle.onDragEnd -= value;
            }
        }

        protected override void SJAwake()
        {
            slider.onValueChanged.AddListener(CallOnValueChangedEvent);
        }

        public void SetValue(float value)
        {
            slider.value = value;
        }

        private void CallOnValueChangedEvent(float value)
        {
            if (onValueChanged != null)
            {
                onValueChanged(value);
            }
        }
    }
}


