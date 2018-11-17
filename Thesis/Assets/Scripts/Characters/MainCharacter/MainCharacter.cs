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

    public override SaveData Save()
    {
        SaveData data = new SaveData(ClassName, true);

        data.AddValue("x", transform.position.x);
        data.AddValue("y", transform.position.y);

        return data;
    }

    public override void Load(SaveData data)
    {
        transform.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));
    }
}
