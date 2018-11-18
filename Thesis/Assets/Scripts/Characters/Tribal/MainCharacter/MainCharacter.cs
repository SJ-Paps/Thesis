using UnityEngine;
using System.Collections.Generic;

public class MainCharacter : Tribal
{

    protected override void Awake()
    {
        base.Awake();

        gameObject.layer = Reg.playerLayer;

        GameManager.GetInstance().GetPlayer();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveLoadManager.GetInstance().SaveGame();
        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            SaveLoadManager.GetInstance().LoadGame();
        }
    }

    public override void GetEnslaved()
    {
        enslaved = true;
    }

    protected override void OnSave(SaveData data)
    {
        data.AddValue("x", transform.position.x);
        data.AddValue("y", transform.position.y);
    }

    protected override void OnLoad(SaveData data)
    {
        transform.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));
    }
}
