using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : SJMonoBehaviour {

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                GameObject obj;

                if (instance == null)
                {
                    obj = new GameObject("GameManager");

                    instance = obj.AddComponent<GameManager>();
                }

                instance.Init();
            }

            return instance;
        }
    }

    public static GameManager GetInstance()
    {
        return Instance;
    }

    public bool IsPaused { get; private set; }

    private void Init()
    {

    }

    void Update()
    {
        CheckPause();
    }

    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(!IsPaused);
        }
    }

    public void Pause(bool shouldPause)
    {
        IsPaused = shouldPause;

        MainMenu.GetInstance().Show(shouldPause);
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
