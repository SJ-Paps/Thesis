using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class SaveLoadManager
{
    private static SaveLoadManager instance;

    public static SaveLoadManager GetInstance()
    {
        if(instance == null)
        {
            instance = new SaveLoadManager();
        }

        return instance;
    }

    public void SaveGame()
    {
        ISaveable[] saveables = GameObject.FindObjectsOfType<SJMonoBehaviour>().OfType<ISaveable>().ToArray();
        
        SaveData[] saveData = new SaveData[saveables.Length];

        for(int i = 0; i < saveData.Length; i++)
        {
            saveData[i] = saveables[i].Save();
        }

        Serialize(saveData);
    }

    public void LoadGame()
    {
        ISaveable[] saveables = GameObject.FindObjectsOfType<SJMonoBehaviour>().OfType<ISaveable>().ToArray();

        saveables[0].Load(Deserialize(Path.Combine(Application.persistentDataPath, "save.sj"))[0]);
    }

    private void Serialize(SaveData[] saves)
    {
        string json = JsonConvert.SerializeObject(saves);

        File.WriteAllText(Path.Combine(Application.persistentDataPath, "save.sj"), json);
    }

    private SaveData[] Deserialize(string path)
    {
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "save.sj"));

        return (SaveData[])JsonConvert.DeserializeObject<SaveData[]>(json);
    }
    
}
