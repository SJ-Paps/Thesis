using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager {

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }

    public Character Player { get; private set; }

    private GameManager()
    {
        Player = FindPlayer();

        Player.onDead += OnPlayerDead;
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

    private void OnPlayerDead()
    {
        SceneManager.LoadScene(0);
    }
}
