using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationInitializer : MonoBehaviour
{

    void Start()
    {
        SceneManager.LoadScene("Menu");
    }
}
