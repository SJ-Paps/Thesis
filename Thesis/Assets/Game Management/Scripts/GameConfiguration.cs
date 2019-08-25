using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct GameConfiguration
{
    [SerializeField]
    public float musicVolume, soundsVolume, generalVolume;

    [SerializeField]
    public string defaultLanguage;
}
