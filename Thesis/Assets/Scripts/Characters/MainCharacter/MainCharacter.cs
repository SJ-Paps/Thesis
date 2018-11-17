using UnityEngine;
using System.Collections.Generic;

public class MainCharacter : Tribal, ISaveable
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

    public SaveData Save()
    {
        SaveData data = new SaveData(GetType().Name);

        data.AddValue("x", transform.position.x.ToString(System.Globalization.CultureInfo.InvariantCulture));
        data.AddValue("y", transform.position.y.ToString(System.Globalization.CultureInfo.InvariantCulture));

        return data;
    }

    public void Load(SaveData data)
    {
        float x = data.GetAs<float>("x");
        float y = data.GetAs<float>("y");

        Debug.Log("MY X: " + x);
        Debug.Log("MY Y: " + y);

        Vector2 pos = new Vector2(x, y);

        transform.position = pos;

        Debug.Log("MY POS: " + pos);
    }

    public void PostSaveCallback()
    {
        
    }

    public void PostLoadCallback()
    {
        
    }
}
