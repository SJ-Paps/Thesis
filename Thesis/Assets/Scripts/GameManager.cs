﻿using UnityEngine;

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
        Player = GameObject.FindObjectOfType<MainCharacter>();
    }
}
