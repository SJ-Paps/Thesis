﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SJ
{
    [Serializable]
    public class GameSettings
    {
        [SerializeField]
        [Range(0.0f, 1.0f)]
        public float musicVolume, soundsVolume, generalVolume;

        [SerializeField]
        public string userLanguage;

        public string lastProfile;
    }
}