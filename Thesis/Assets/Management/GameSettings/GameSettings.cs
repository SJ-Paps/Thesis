using UnityEngine;

namespace SJ.Management
{
    [CreateAssetMenu(menuName = "SJ/Game Settings Asset")]
    public class GameSettings : ScriptableObject
    {
        [Range(0.0f, 1.0f)]
        public float musicVolume, effectsVolume, generalVolume;

        [SerializeField]
        public string userLanguage;

        [HideInInspector]
        public string lastProfile;
    }
}