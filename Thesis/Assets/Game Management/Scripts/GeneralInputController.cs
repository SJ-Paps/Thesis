﻿using Paps.Unity;
using UnityEngine;

public class GeneralInputController : MonoBehaviour
{
    private static GeneralInputController instance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        GameManager.GetInstance().onLoadingSucceeded += InstantiateInGame;
        GameManager.GetInstance().onQuitting += DestroyOnQuittingGame;
    }

    private static void DestroyOnQuittingGame()
    {
        UnityUtil.DestroyDontDestroyOnLoadObject(instance.gameObject);
    }

    private static void InstantiateInGame()
    {
        instance = new GameObject(nameof(GeneralInputController)).AddComponent<GeneralInputController>();

        UnityUtil.DontDestroyOnLoad(instance.gameObject);
    }

    private void Awake()
    {
        MainMenu.GetInstance().Hide();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(MainMenu.GetInstance().Shown)
            {
                MainMenu.GetInstance().Hide();
            }
            else
            {
                MainMenu.GetInstance().Show();
            }
        }
    }
}
