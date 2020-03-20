using SJ.Management;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SJ
{
    public class ApplicationInitializer : MonoBehaviour
    {
        [SerializeField]
        private LoadAction[] loadActions;

        void Awake()
        {
            Application.Initialize(loadActions);
        }
    }
}