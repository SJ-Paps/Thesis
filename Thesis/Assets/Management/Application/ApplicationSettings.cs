using System;
using UnityEngine;

namespace SJ
{
    [Serializable]
    public class ApplicationSettings
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