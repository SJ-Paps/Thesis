using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ
{
    [CreateAssetMenu(fileName = "ApplicationInfoAsset", menuName = "Application Info Asset")]
    public class ApplicationInfoAsset : ScriptableObject
    {
        [SerializeField]
        private string[] beginningScenes;

        [SerializeField]
        private string returnSceneOnEndSession;

        [SerializeField]
        private GameSettings defaultGameConfiguration;

        public string[] BeginningScenes { get { return beginningScenes; } }

        public string ReturnSceneOnEndSession { get { return returnSceneOnEndSession; } }

        public GameSettings DefaultGameConfiguration { get { return defaultGameConfiguration; } }
    }
}