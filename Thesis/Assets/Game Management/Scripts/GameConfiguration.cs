using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct GameConfiguration
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    public float musicVolume, soundsVolume, generalVolume;

    [SerializeField]
    public string userLanguage;

    public string lastProfile;
}
