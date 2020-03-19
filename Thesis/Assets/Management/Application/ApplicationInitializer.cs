using UnityEngine;
using UnityEngine.SceneManagement;

namespace SJ
{
    public class ApplicationInitializer : MonoBehaviour
    {
        void Awake()
        {
            Application.OnInitialized += () => SceneManager.LoadScene("Menu");
            Application.Initialize();
        }
    }
}