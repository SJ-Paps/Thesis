using UnityEngine;

namespace SJ.Management
{
    [CreateAssetMenu(fileName = "ApplicationSettings", menuName = "Application Settings Asset")]
    public class ApplicationSettings : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private string returnSceneOnEndSession, baseScene;

        [Space(10)]
        [SerializeField]
        private string[] beginningScenes, ignoreScenesOnSave;

        public string[] BeginningScenes { get; private set; }
        public string[] IgnoreScenesOnSave { get; private set; }
        public string ReturnSceneOnEndSession { get => returnSceneOnEndSession; }
        public string BaseScene { get => baseScene; }

        public void OnAfterDeserialize()
        {
            BeginningScenes = new string[beginningScenes.Length];
            beginningScenes.CopyTo(BeginningScenes, 0);

            IgnoreScenesOnSave = new string[ignoreScenesOnSave.Length];
            ignoreScenesOnSave.CopyTo(IgnoreScenesOnSave, 0);
        }

        public void OnBeforeSerialize()
        {
            
        }
    }

}