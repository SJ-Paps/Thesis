using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSingleMode : ScriptableLoadRoutine
{
    [SerializeField]
    private string sceneName;

    public override bool IsCompleted
    {
        get
        {
            return true;
        }
    }

    public override bool IsFaulted
    {
        get
        {
            return false;
        }
    }

    public override void Load()
    {
        SceneManager.LoadScene(sceneName);
    }

    public override bool ShouldRetry()
    {
        return false;
    }
}
