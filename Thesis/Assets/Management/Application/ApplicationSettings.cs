using UnityEngine;

namespace SJ.Management
{
    [CreateAssetMenu(fileName = "ApplicationSettings", menuName = "Application Settings Asset")]
    public class ApplicationSettings : ScriptableObject
    {
        [SerializeField]
        private string[] beginningScenes;
        [SerializeField]
        private string returnSceneOnEndSession;

        public string[] BeginningScenes { get => beginningScenes; }
        public string ReturnSceneOnEndSession { get => returnSceneOnEndSession; }
    }

}