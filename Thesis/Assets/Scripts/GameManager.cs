using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Character player;

    public bool IsPaused { get; private set; }

    private void Init()
    {
        DontDestroyOnLoad(this);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public Character GetPlayer()
    {
        if (player == null)
        {
            player = FindPlayer();
            PreparePlayer(player);
        }

        return player;
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
        if(player != null)
        {
            return player;
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
            player.onDead += OnPlayerDead;
        }
    }

    private void OnPlayerDead()
    {
        SceneManager.LoadScene(0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = null;
    }
}
