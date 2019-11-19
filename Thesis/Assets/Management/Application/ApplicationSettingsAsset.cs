using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ
{
    [CreateAssetMenu(fileName = "ApplicationSettingsAsset", menuName = "Application Settings Asset")]
    public class ApplicationSettingsAsset : ScriptableObject
    {
        [SerializeField]
        private ApplicationSettings applicationSettings;

        public ApplicationSettings GetApplicationSettings()
        {
            return applicationSettings;
        }
    }
}