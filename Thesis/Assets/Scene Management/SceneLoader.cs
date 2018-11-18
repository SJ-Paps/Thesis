using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader
{
    private static SceneLoader instance;

    public static SceneLoader GetInstance()
    {
        if(instance == null)
        {
            instance = new SceneLoader();
        }

        return instance;
    }

    public void NewGame()
    {

    }

    public void LoadGame()
    {
        SaveData[] saves = SaveLoadManager.GetInstance().LoadGame();


    }
}
