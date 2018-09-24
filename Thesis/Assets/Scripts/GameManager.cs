using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(!init)
            {
                init = true;
                instance = Instantiate<GameManager>(SJResources.Instance.LoadGameObjectAndGetComponent<GameManager>("GameManager"));
                instance.Init();
            }

            return instance;
        }
    }

    public static GameManager GetInstance()
    {
        return Instance;
    }

    private static bool init;

    public Character Player { get; private set; }

    public bool IsPaused { get; private set; }

    private void Init()
    {
        DontDestroyOnLoad(this);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
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

    public Character FindPlayer()
    {
        if(Player != null)
        {
            return Player;
        }

        foreach (Character obj in GameObject.FindObjectsOfType<Character>())
        {
            if (obj.gameObject.layer == Reg.playerLayer)
            {
                return obj;
            }
        }

        return null;
    }

    private void PreparePlayer(Character player)
    {
        if(player != null)
        {
            Player.onDead += OnPlayerDead;
        }
    }

    private void OnPlayerDead()
    {
        SceneManager.LoadScene(0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = FindPlayer();
        PreparePlayer(Player);
    }
}
