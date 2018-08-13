using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class SoundSlider : MonoBehaviour {

    [SerializeField]
    private Slider slider;

    public void SetData(UnityAction<float> onValueChanged, float initValue)
    {
        slider.onValueChanged.AddListener(onValueChanged);

        slider.value = initValue;
    }
}
