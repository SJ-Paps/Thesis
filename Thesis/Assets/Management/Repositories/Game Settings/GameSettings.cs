using System;
using UnityEngine;

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

        [HideInInspector]
        public string lastProfile;
    }
}