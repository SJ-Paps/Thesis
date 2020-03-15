using System;
using UnityEngine;

namespace SJ
{
    [CreateAssetMenu(fileName = "ApplicationSettings", menuName = "Application Settings Asset")]
    public class ApplicationSettings : ScriptableObject
    {
        [SerializeField]
        private string[] beginningScenes;
        [SerializeField]
        private string returnSceneOnEndSession;
        [SerializeField]
        private GameSettings defaultGameSettings;

        public string[] BeginningScenes { get => beginningScenes; }
        public string ReturnSceneOnEndSession { get => returnSceneOnEndSession; }
        public GameSettings DefaultGameSettings { get => defaultGameSettings; }
    }

}