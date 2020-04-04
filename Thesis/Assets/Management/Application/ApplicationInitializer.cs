using UnityEngine;

namespace SJ.Management
{
    public class ApplicationInitializer : MonoBehaviour
    {
        [SerializeField]
        private LoadAction[] loadActions;

        void Awake()
        {
            Application.Instance.Initialize(loadActions);
        }
    }
}